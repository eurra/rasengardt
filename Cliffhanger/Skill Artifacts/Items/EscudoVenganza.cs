// Escudo de la Venganza.
// Hecho por Cliffhanger (archon.cl@gmail.com)

using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Spells;
using Server.Targeting;
using Server.Regions;
using Server.Scripts.Commands;

namespace Server.Items
{
	public class EscudoVenganza : BaseShield, SkillArtifact
	{
                private bool m_VenganceMode = false;
                private int PuntosAc = 0;
                private int m_TipoDanoLiberado = 0;
                private DateTime LastCambioModo = DateTime.Now;

                public static TimeSpan delay = TimeSpan.FromSeconds( 30 );
                //private IHumHueTimer huetimer;
                
                private Type m_BaseType;
		private int[] BaseShieldValues = new int[4];

		public bool VenganceMode{ get{ return m_VenganceMode; } }
		public int TipoDanoLiberado{ get{ return m_TipoDanoLiberado; } }

		public override int InitMinHits{ get{ return BaseShieldValues[0]; } }
		public override int InitMaxHits{ get{ return BaseShieldValues[1]; } }

		public override int AosStrReq{ get{ return BaseShieldValues[2]; } }

		public override int ArmorBase{ get{ return BaseShieldValues[3]; } }

		public override int BasePhysicalResistance{ get{ return 4; } }
		public override int BaseFireResistance{ get{ return 4; } }
		public override int BaseColdResistance{ get{ return 4; } }
		public override int BasePoisonResistance{ get{ return 0; } }
		public override int BaseEnergyResistance{ get{ return 0; } }

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
                        		m_TipoBase = typeof(HeaterShield);
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

  			BaseShieldValues[0] = armor.InitMinHits;
			BaseShieldValues[1] = armor.InitMaxHits;
			BaseShieldValues[2] = armor.AosStrReq;
			BaseShieldValues[3] = armor.ArmorBase;

			Weight = armor.Weight;
			Layer = armor.Layer;
			
			armor.Delete();
    		}
		
		[Constructable]
		public EscudoVenganza() : this( SAUtility.TipoBaseRandom( typeof(EscudoVenganza) ) )
		{
		}
		
		[Constructable]
		public EscudoVenganza( Type tiposhield ) : base( 0x13B )
		{
			TipoBase = tiposhield;
			SetProps( null );
  		}

		public EscudoVenganza( BaseShield ShieldBase ) : base( 0x13B )
		{
			Type tipo = null;

			if( ShieldBase != null )
				tipo = ShieldBase.GetType();

			TipoBase = tipo;
			SetProps( ShieldBase as BaseArmor );
		}

                public EscudoVenganza( Serial serial ) : base( serial )
		{
		}
		
 		private void SetProps( BaseArmor ArmorBase )
		{
			if( ArmorBase != null )
                	{
				SAUtility.CopyAosAttributes( ArmorBase, this as BaseArmor );

                        	//AosAttribute
                        	if( Attributes.AttackChance > 10 )
                        		Attributes.AttackChance = 10;
                        	if( Attributes.DefendChance > 10 )
                        		Attributes.DefendChance = 10;
                	}
                	
                	Hue = 193;
                        Name = SAUtility.NombreArtefacto( GetType() );
                        Attributes.ReflectPhysical = 20;
                        Attributes.SpellChanneling = 0;
                        Attributes.CastSpeed = 1;
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
                        	from.SendMessage("Debes tener equipado el Escudo para usarlo.");
                        	return;
                        }

			if( ((dovPlayerMobile)from).EVLastDobleClick + delay > DateTime.Now )
			{
                        	from.SendMessage("Debes esperar 30 segundos para volver a usar el Escudo.");
                        	return;
                        }
                        
                        if ( !SkillRequerida(from) )
                        {
                        	from.SendMessage("Necesitas 80 de Parrying para activar el escudo.");
                        	return;
                        }
                        
                        if ( from.Mana < 10 )
                        {
                        	from.SendMessage("Necesitas 10 de mana para usar el Escudo!");
                        	return;
                        }

                        if ( m_VenganceMode )
                        {
                        	from.SendMessage("El Escudo ya esta activado!");
                        	return;
                        }

                        if( PuntosAc == 0 )
			{
                        	from.SendMessage("El Escudo no tiene puntos de daño Acumulados!");
                        	return;
                        }
                        
                        if ( from.Region is Jail )
                        {
                        	from.SendMessage("No puedes usar el escudo aca.");
                        	return;
                        }
                        
                        from.RevealingAction();
			SetVenganceMode( from );

		}
		
		private void SetVenganceMode( Mobile from )
		{
			((dovPlayerMobile)from).EVLastDobleClick = DateTime.Now;
			from.LocalOverheadMessage( MessageType.Regular, 193, true, "El Escudo libera su poder!");
                	from.PlaySound( 903 );
                	from.FixedParticles( 14201, 1, 25, 9961, 193, 0, EffectLayer.LeftHand );
			//huetimer = new IHumHueTimer( this as Item );
                	//huetimer.Start();
			m_VenganceMode = true;
		}

		public void LiberarDam( Mobile from, Mobile to )
		{
			to.FixedParticles( 0x37C4, 10, 30, 9961, 193, 0, EffectLayer.LeftFoot );
			from.PlaySound( 0x210 );
			from.Mana -= 10;
                        to.SendMessage("El Escudo Humano te ha provocado daño extra!");
                        to.Damage( PuntosAc, from );
                        PuntosAc = 0;
                        InvalidateProperties();
                        m_VenganceMode = false;
                        //huetimer.Stop();
                        //Hue = 193;
		}
		
		public void AbsDamage( int damage )
		{
                        Mobile owner = Parent as Mobile;

			if ( ( owner != null && !SkillRequerida( owner ) ) || m_VenganceMode || PuntosAc == 20 || damage == 0 )
                        	return;

                        int abs = damage / 8;
                        
                        if ( abs < 1 )
                        	abs = 1;

                        if ( PuntosAc + abs > 20 )
                        	PuntosAc = 20;
                        else
                        	PuntosAc += abs;

                        InvalidateProperties();

			owner.FixedParticles( 0x37BE, 1, 15, 9961, 193, 0, EffectLayer.Head );
		}

		public bool SkillRequerida( Mobile m )
		{
			if ( m.Skills.Parry.Value < 80.0 )
				return false;
			
			return true;
		}
		
/************************************/
/******* HABILIDAD SECUNDARIA *******/
/************************************/

		public void Switch( Mobile m )
                {
                	if ( LastCambioModo + TimeSpan.FromSeconds( SACommands.DelaySwitch ) > DateTime.Now )
			{
                            	m.SendMessage("Debes esperar 3 segundos para cambiar el tipo de daño absorbido.");
				return;
			}
			
			if ( !SkillRequerida(m) )
                        {
                        	m.SendMessage("Necesitas 80 de Parrying para activar el escudo.");
                        	return;
                        }

			CambiarTipoDano( m );
                }
                
                private void CambiarTipoDano ( Mobile m )
		{
                	
			switch ( m_TipoDanoLiberado )
			{
				
				case 0:
				{
					m_TipoDanoLiberado = 1;
                                        m.LocalOverheadMessage( MessageType.Regular, 193, true, "Ahora el Escudo absorbe ataques magicos");
                                        Attributes.CastSpeed = 0;
                                        Attributes.SpellChanneling = 1;
                                        break;
    				}
				case 1:
				{
				        m_TipoDanoLiberado = 0;
					m.LocalOverheadMessage( MessageType.Regular, 193, true, "Ahora el Escudo absorbe ataques fisicos");
					Attributes.CastSpeed = 1;
                                        Attributes.SpellChanneling = 0;
					break;
   				}
   			}
   			
   			if( PuntosAc > 0 )
   				PuntosAc = 0;
   				
   			if ( m_VenganceMode )
			{
				m_VenganceMode = false;
				//huetimer.Stop();
				//Hue = 193;
			}

                	m.PlaySound( 0x1EA );
                	m.FixedParticles( 0x376A, 1, 15, 9961, 193, 0, EffectLayer.LeftHand );
                	InvalidateProperties();

                }

/*****************************************/
/******* OTROS METODOS ADICIONALES *******/
/*****************************************/
                
                public override void OnRemoved( object parent )
                {
			if ( m_VenganceMode )
			{
				m_VenganceMode = false;
				//huetimer.Stop();
				//Hue = 193;
			}
                        
                        base.OnRemoved( parent );
		}
		
		public override bool OnEquip( Mobile from )
		{
			if( SAUtility.MuchosEquipados( from ) )
                        {
                        	from.SendMessage( "No puedes equiparte otro artefacto." );
                        	return false;
                        }
                        
                        return base.OnEquip( from );
                }

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );
			list.Add( 1060660, "Daño Absorbido: {0}\nPtos. Acumulados\t" + PuntosAc.ToString(), m_TipoDanoLiberado == 0 ? "Físico" : "Mágico" );
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
			
			TipoBase = ScriptCompiler.FindTypeByName( reader.ReadString() );

			this.Hue = 193;
		}

	}

}
