// Guantes de la Vitalidad.
// Hecho por Cliffhanger (archon.cl@gmail.com)

using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Spells;
using Server.Targeting;
using Server.Scripts.Commands;

namespace Server.Items
{
	
	public class GuantesVitalidad : BaseArmor, SkillArtifact
	{
                private int StatMode = 0;

                public static TimeSpan delay = TimeSpan.FromSeconds( 15 );
                private DateTime LastCambioModo = DateTime.Now;
                
                private static int FocusBonus = 10;                
                private SkillMod smod = new DefaultSkillMod( SkillName.Focus, true, FocusBonus );
                
                public override int BasePhysicalResistance{ get { return 10; } }
                public override int BaseFireResistance{ get { return 10; } }
                public override int BaseColdResistance{ get { return 10; } }
                public override int BasePoisonResistance{ get { return 5; } }
                public override int BaseEnergyResistance{ get { return 5; } }

                private ArmorMaterialType m_MaterialType;
                private int[] BaseGlovesValues = new int[5];

		public override int InitMinHits{ get{ return BaseGlovesValues[0]; } }
		public override int InitMaxHits{ get{ return BaseGlovesValues[1]; } }

		public override int AosStrReq{ get{ return BaseGlovesValues[2]; } }
		public override int OldStrReq{ get{ return BaseGlovesValues[3]; } }

		public override int ArmorBase{ get{ return BaseGlovesValues[4]; } }
		
		public override ArmorMaterialType MaterialType{ get{ return m_MaterialType; } }

/*************************************************/
/******* CONSTRUCTORES Y METODOS ASOCIADOS *******/
/*************************************************/

		private Type m_TipoBase;

		[CommandProperty( AccessLevel.GameMaster )]
		public Type TipoBase
  		{
  			get{ return m_TipoBase; }

  			set
  			{
    				if( value == null || !SAUtility.EsTipoBaseValido( this.GetType(), value ) )
                        		m_TipoBase = typeof(LeatherGloves);
				else
                        		m_TipoBase = value;

				ConfigurarTipoBase();
				InvalidateProperties();
			}
		}

		private void ConfigurarTipoBase()
		{
			BaseArmor armor = Activator.CreateInstance( m_TipoBase ) as BaseArmor;

			if( armor == null )
				return;

  			ItemID = armor.ItemID;
  				
			BaseGlovesValues[0] = armor.InitMinHits;
			BaseGlovesValues[1] = armor.InitMaxHits;

			BaseGlovesValues[2] = armor.AosStrReq;
			BaseGlovesValues[3] = armor.OldStrReq;

			BaseGlovesValues[4] = armor.ArmorBase;
			
			m_MaterialType = armor.MaterialType;

			Weight = armor.Weight;
			Layer = armor.Layer;

			armor.Delete();
		}
		
		[Constructable]
		public GuantesVitalidad() : this( SAUtility.TipoBaseRandom( typeof(GuantesVitalidad) ) )
		{
		}
		
		[Constructable]
		public GuantesVitalidad( Type tipoguantes ) : base( 0x13B )
		{
			TipoBase = tipoguantes;
			SetProps( null );
  		}
  		
		public GuantesVitalidad( BaseArmor GuantesBase ) : base( 0x13B )
		{
			Type tipo = null;
			
			if( GuantesBase != null )
				tipo = GuantesBase.GetType();

			TipoBase = tipo;
			SetProps( GuantesBase as BaseArmor );
		}

                public GuantesVitalidad( Serial serial ) : base( serial )
		{
		}

  		private void SetProps( BaseArmor ArmorBase )
		{
			if( ArmorBase != null )
                	{
				SAUtility.CopyAosAttributes( ArmorBase, this as BaseArmor );

                        	//AosAttribute
                        	if( Attributes.RegenMana > 1 )
                          		Attributes.RegenMana = 1;
                        	if( Attributes.RegenStam > 1 )
                          		Attributes.RegenStam = 1;
                	}
                	
                	Hue = 1269;
                        Name = SAUtility.NombreArtefacto( GetType() );
                        ArmorAttributes.MageArmor = 1;
                        Attributes.RegenHits = 2;
		}

/**********************************/
/******* HABILIDAD PRIMARIA *******/
/**********************************/

		public override void OnDoubleClick( Mobile from )
		{
			if ( !(from is dovPlayerMobile) )
				return;

			if ( Parent != from )
                        {
                        	from.SendMessage("Debes tener equipados los Guantes para usarlos.");
                        	return;
                        }

			if( ((dovPlayerMobile)from).GVLastDobleClick + delay > DateTime.Now )
			{
                        	from.SendMessage("Debes esperar 20 segundos antes de usar los Guantes.");
                        	return;
                        }
                        
                        if ( !SkillRequerida(from) )
                        {
                        	from.SendMessage("Necesitas 80 de Focus para usar los guantes.");
                        	return;
                        }
                        
                        if( from.Poisoned || from.YellowHealthbar )
                        {
                        	from.SendMessage("No puedes usar los guantes en tu estado actual.");
                        	return;
                        }

			from.RevealingAction();
			SacrificarStat( from );

		}
		
		public bool SkillRequerida( Mobile m )
                {
                	if ( m.Skills.Focus.Value < 80.0 )
				return false;
			
			return true;
                }
		
		private void SacrificarStat( Mobile from )
		{

			// Cantidad de stats cambiados
			int s1 = 20;
			int s2 = 20;

			switch ( StatMode )
			{
				case 0:
				{
					if( from.Stam < s1 )
						s1 = from.Stam;

					if( from.Mana < s2 )
						s2 = from.Mana;

    					from.Stam -= s1;
    					from.Mana -= s2;

    					if( (from.Hits + ((s1 + s2) / 2) ) > from.HitsMax )
						from.Hits = from.HitsMax;
					else
                              			from.Hits += (s1 + s2) / 2;
                              		
                              		from.PlaySound( 0x202 );
                			from.FixedParticles( 0x373A, 1, 15, 9961, 1269, 0, EffectLayer.LeftHand );

                              		break;
   				}
				case 1:
				{
					if( from.Hits < s1 )
						s1 = from.Hits - 1;

					if( from.Mana < s2 )
						s2 = from.Mana;

    					from.Hits -= s1;
    					from.Mana -= s2;
    				
    					if( (from.Stam + ((s1 + s2) / 2) ) > from.Dex )
						from.Stam += from.Dex;
					else
                                        	from.Stam += (s1 + s2) / 2;
                                        	
                                        from.PlaySound( 0x202 );
                			from.FixedParticles( 0x373A, 1, 15, 9961, 1269, 0, EffectLayer.LeftHand );

                                        break;
    				}
				case 2:
				{
					if( from.Hits < s1 )
						s1 = from.Hits - 1;

					if( from.Stam < s2 )
						s2 = from.Stam;

    					from.Hits -= s1;
    					from.Stam -= s2;
    				
    					if( (from.Mana + ((s1 + s2) / 2)) > from.Int )
						from.Mana += from.Int;
					else
                                        	from.Mana += (s1 + s2) / 2;
                                        	
                                        from.PlaySound( 0x202 );
                			from.FixedParticles( 0x373A, 1, 15, 9961, 1269, 0, EffectLayer.LeftHand );
                                        	
                                        break;
   				}
   			}
   			
   			((dovPlayerMobile)from).GVLastDobleClick = DateTime.Now;

  		}

/************************************/
/******* HABILIDAD SECUNDARIA *******/
/************************************/

		public void Switch( Mobile m )
		{
			if ( LastCambioModo + TimeSpan.FromSeconds( SACommands.DelaySwitch ) > DateTime.Now )
			{
                            	m.SendMessage("Debes esperar 3 segundos para cambiar el Stat Elegido.");
				return;
			}
			
			if ( !SkillRequerida(m) )
                        {
                        	m.SendMessage("Necesitas 80 de Focus para usar los guantes.");
                        	return;
                        }
				
			SwitchStat( m );
  		}

		private void SwitchStat ( Mobile m )
		{
			switch ( StatMode )
			{
				
				case 0:
				{
					StatMode = 1;
                                        m.LocalOverheadMessage( MessageType.Regular, 1269, true, "Tus Guantes te daran Agilidad");
                                        break;
   				}
				case 1:
				{
					StatMode = 2;
					m.LocalOverheadMessage( MessageType.Regular, 1269, true, "Tus Guantes te daran Mana");
					break;
    				}
				case 2:
				{
					StatMode = 0;
					m.LocalOverheadMessage( MessageType.Regular, 1269, true, "Tus Guantes te daran Vida");
					break;
   				}
   			}

                	m.PlaySound( 0x1EA );
                	m.FixedParticles( 0x376A, 1, 15, 9961, 1269, 0, EffectLayer.LeftHand );
                	InvalidateProperties();

                }

/*****************************************/
/******* OTROS METODOS ADICIONALES *******/
/*****************************************/

		private void SetearSkillsDef( Mobile m )
		{
			if ( m == null )
				return;

                        m.AddSkillMod( smod );
		}

                public override bool OnEquip( Mobile from )
                {
                        if( SAUtility.MuchosEquipados( from ) )
                        {
                        	from.SendMessage( "No puedes equiparte otro artefacto." );
                        	return false;
                        }

			SetearSkillsDef( from );

                	return base.OnEquip( from );
                }

                public override void OnRemoved( object parent )
                {
                        if ( smod != null )
				smod.Remove();

                        base.OnRemoved( parent );
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			String final = "Bonus Focus: " + FocusBonus.ToString() + "\nStat Elegido\t";

			if( StatMode == 0 )
                                final += "Hits";
			else if ( StatMode == 1 )
				final += "Stamina";
                        else if ( StatMode == 2 )
				final += "Mana";

			list.Add( 1060658, final );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
			
			writer.Write( m_TipoBase.Name );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
			
			SetearSkillsDef( Parent as Mobile );
			
			TipoBase = ScriptCompiler.FindTypeByName( reader.ReadString() );

                        Resource = CraftResource.None;
			this.Hue = 1269;
		}
	}
}
