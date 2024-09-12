// Amuleto de la Proteccion.
// Hecho por Cliffhanger (archon.cl@gmail.com)

using System;
using Server.Network;
using Server.Items;
using Server.Mobiles;
using Server.Spells;
using Server.Targeting;
using Server.Regions;
using Server.Scripts.Commands;

namespace Server.Items
{
	public class AmuletoProteccion : BaseNecklace, SkillArtifact
	{
                private SummonedGolem golem;

                private static int BonusLumber = 10;
                private static int BonusTinker = 10;
                private SkillMod smod1 = new DefaultSkillMod( SkillName.Lumberjacking, true, BonusLumber );
		private SkillMod smod2 = new DefaultSkillMod( SkillName.Tinkering, true, BonusTinker );

                private int SkillSel;
		private SkillMod switchmod1 = new DefaultSkillMod( SkillName.Blacksmith, true, 0 );
		private SkillMod switchmod2 = new DefaultSkillMod( SkillName.Mining, true, 0 );

		public static TimeSpan delay = TimeSpan.FromMinutes( 5 );
		private DateTime LastCambioModo = DateTime.Now;
		
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
                        		m_TipoBase = typeof(Necklace);
				else
                        		m_TipoBase = value;

				ConfigurarTipoBase();
				InvalidateProperties();
			}
		}

		private void ConfigurarTipoBase()
		{
			Item necklace = Activator.CreateInstance( m_TipoBase ) as Item;

			if( necklace == null )
				return;

  			ItemID = necklace.ItemID;

			Weight = necklace.Weight;
			Layer = necklace.Layer;

			necklace.Delete();
		}
		
		[Constructable]
		public AmuletoProteccion() : this( SAUtility.TipoBaseRandom( typeof(AmuletoProteccion) ) )
		{
		}
		
		[Constructable]
		public AmuletoProteccion( Type tipoamuleto ) : base( 0x13B )
		{
			TipoBase = tipoamuleto;
			SetProps( null );
  		}

		public AmuletoProteccion( BaseNecklace AmuletoBase ) : base( 0x13B )
		{
			Type tipo = null;
			
			if( AmuletoBase != null )
				tipo = AmuletoBase.GetType();

			TipoBase = tipo;
			SetProps( null );
		}

                public AmuletoProteccion( Serial serial ) : base( serial )
		{
		}
		
		private void SetProps( BaseJewel JoyaBase )
		{
			if( JoyaBase != null )
			{
				SAUtility.CopyAosAttributes( JoyaBase, this as BaseJewel );
			        
			        if( Attributes.BonusMana > 5 )
			        	Attributes.BonusMana = 5;
                                
                                GemType = JoyaBase.GemType;
                                Resource = JoyaBase.Resource;
			}

		        Hue = 2111;
                        Name = SAUtility.NombreArtefacto( GetType() );

                        Resistances.Physical = 5;
                        Resistances.Fire = 10;
                        Resistances.Cold = 10;
                        Resistances.Poison = 5;
                	Resistances.Energy = 5;
		        Attributes.DefendChance = 5;
		        SkillSel = 0;
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
                        	from.SendMessage("Debes tener equipado el Amuleto para usarlo.");
                        	return;
                        }

                        if ( from.Mana < 50 )
			{
                        	from.SendMessage ("No tienes suficiente mana para invocar la criatura.");
                                return;
                        }

			if( ((dovPlayerMobile)from).APLastDobleClick + delay > DateTime.Now )
			{
                        	from.SendMessage("Debes esperar 5 minutos para invocar otro Golem.");
                        	return;
                        }
                        
                        if ( from.Region is Jail )
                        {
                        	from.SendMessage("No puedes usar el amuleto aca.");
                        	return;
                        }

			from.RevealingAction();
			SummonGolem( from );
		}

		private void SummonGolem( Mobile master )
		{
   			SummonedGolem summon = new SummonedGolem();
			master.Mana -= 50;
			BaseCreature.Summon( summon, master, master.Location, 0x217, TimeSpan.FromMinutes( 4 ) );
                        summon.FixedParticles( 0x3728, 1, 10, 2111, EffectLayer.Head );
                        golem = summon;
                        summon.Summoned = false;

                        ((dovPlayerMobile)master).APLastDobleClick = DateTime.Now;
                }
                
/************************************/
/******* HABILIDAD SECUNDARIA *******/
/************************************/

		public void Switch( Mobile m )
		{
			if ( !SkillRequerida( m ) )
                        {
                        	m.SendMessage("Esta accion requiere que tengas 100 de BlackSmith y Mining minimo.");
                        	return;
                        }

			if ( LastCambioModo + TimeSpan.FromSeconds( SACommands.DelaySwitch ) > DateTime.Now )
			{
                            	m.SendMessage("Debes esperar 3 segundos para cambiar los Skills.");
				return;
			}

			ActivarSwitch( m );
			LastCambioModo = DateTime.Now;
  		}

		private void ActivarSwitch ( Mobile m )
		{
			switch ( SkillSel )
			{
				case 0:
				{
					SkillSel = 1;
					break;
				}
                                case 1:
				{
					SkillSel = 2;
					break;
				}
				case 2:
				{
					SkillSel = 0;
					break;
				}
			}

			CambiarSkills( m );
			m.LocalOverheadMessage( MessageType.Regular, 2111, true, "El Amuleto cambia tus habilidades!");
			m.FixedParticles( 0x376A, 1, 15, 9961, 2111, 0, EffectLayer.RightHand );
			m.PlaySound( 0x1EF );
			InvalidateProperties();
		}

		private void CambiarSkills( Mobile m )
		{
			if ( m == null )
				return;

			switch ( SkillSel )
			{
				case 0:
    				{
                                        switchmod1.Value = 0;
                                        switchmod2.Value = 0;
                        		break;
        			}
				case 1:
				{
					switchmod1.Value = 10;
                                	switchmod2.Value = -10;
                        		break;
    				}
				case 2:
				{
					switchmod1.Value = -10;
                                	switchmod2.Value = 10;
                        		break;
    				}
   			}
   			
   			m.AddSkillMod( switchmod1 );
                        m.AddSkillMod( switchmod2 );
		}

		public bool SkillRequerida( Mobile m )
		{
			if ( (m.Skills.Blacksmith.Value < 100.0) || (m.Skills.Mining.Value < 100.0) )
				return false;

			return true;
		}

/*****************************************/
/******* OTROS METODOS ADICIONALES *******/
/*****************************************/

		private void SetearSkillsDef( Mobile m )
		{
			if ( m == null )
				return;

                        m.AddSkillMod( smod1 );
                        m.AddSkillMod( smod2 );
		}

		private void SacarSkillMods( Mobile from )
		{
			if ( smod1 != null )
				smod1.Remove();
			if ( smod2 != null )
				smod2.Remove();
			if ( switchmod1 != null )
				switchmod1.Remove();
			if ( switchmod2 != null )
				switchmod2.Remove();
		}

                public override bool OnEquip( Mobile from )
                {
                        if( SAUtility.MuchosEquipados( from ) )
                        {
                        	from.SendMessage( "No puedes equiparte otro artefacto." );
                        	return false;
                        }

			SetearSkillsDef( from );
			
			if( SkillRequerida( from ) )
                        	CambiarSkills( from );

                	return base.OnEquip( from );

                }

                public override void OnRemoved( object parent )
                {
			if ( !(parent is Mobile) )
				return;

                        SacarSkillMods( (Mobile)parent );

                        if ( golem != null )
                        {
                        	try
                        	{
                        		golem.Dispel( golem );
                        	}
                        	catch
                        	{
                        	}

                        	golem = null;
                        }

                        base.OnRemoved( parent );
		}

                public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );
			
			String final = "Bonus Lumberjacking: " + BonusLumber.ToString() + "\nBonus Tinkering: " + BonusTinker.ToString() + "\nIntercambio Skills\t";
			
			if( SkillSel == 0 )
                        	final += "Ninguno";
                        else if( SkillSel == 1 )
                                final += "Mining a BS";
                        else if( SkillSel == 2 )
                                final += "BS a Mining";

			list.Add( 1060660, final );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
			
                        writer.Write( (int) SkillSel );
                        
                        writer.Write( m_TipoBase.Name );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			SkillSel = reader.ReadInt();
			TipoBase = ScriptCompiler.FindTypeByName( reader.ReadString() );

			CambiarSkills( Parent as Mobile );
			SetearSkillsDef( Parent as Mobile );

			this.Hue = 2111;
		}
	}
}
