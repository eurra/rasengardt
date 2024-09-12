// Traje de los Elementos.
// Hecho por Cliffhanger (archon.cl@gmail.com)

using System;
using Server.Network;
using Server.Items;
using Server.Mobiles;
using Server.Spells;
using Server.Regions;
using Server.Scripts.Commands;

namespace Server.Items
{
	public class TrajeElementos : BaseOuterTorso, SkillArtifact
	{
                private int ResistAct = 0;

                public static TimeSpan delay = TimeSpan.FromSeconds( 20 );
                private DateTime LastCambioModo = DateTime.Now;

                private static int BonusMagery = 10;
		private SkillMod smod1 = new DefaultSkillMod( SkillName.Magery, true, BonusMagery );
                
                private ResistanceMod rmod1 = new ResistanceMod( ResistanceType.Physical, 0 );
                private ResistanceMod rmod2 = new ResistanceMod( ResistanceType.Fire, 0 );
                private ResistanceMod rmod3 = new ResistanceMod( ResistanceType.Cold, 0 );
                private ResistanceMod rmod4 = new ResistanceMod( ResistanceType.Poison, 0 );
                private ResistanceMod rmod5 = new ResistanceMod( ResistanceType.Energy, 0 );
                
		public override int PhysicalResistance{ get{ return 0; } }
		public override int FireResistance{ get{ return 0; } }
		public override int ColdResistance{ get{ return 5; } }
		public override int PoisonResistance{ get{ return 0; } }
		public override int EnergyResistance{ get{ return 0; } }

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
                        		m_TipoBase = typeof(Robe);
				else
                        		m_TipoBase = value;

				ConfigurarTipoBase();
				InvalidateProperties();
			}
		}

		private void ConfigurarTipoBase()
		{
			Item robe = Activator.CreateInstance( m_TipoBase ) as Item;

			if( robe == null )
				return;

  			if( m_TipoBase == typeof( HoodedRobe ) )
                        	ItemID = 0x2683;
                        else
				ItemID = robe.ItemID;

			Weight = robe.Weight;
			Layer = robe.Layer;

			robe.Delete();
		}
		
		[Constructable]
		public TrajeElementos() : this( SAUtility.TipoBaseRandom( typeof(TrajeElementos) ) )
		{
		}
		
		[Constructable]
		public TrajeElementos( Type tiporobe ) : base( 0x13B )
		{
			TipoBase = tiporobe;
			SetProps( null );
  		}

		public TrajeElementos( BaseOuterTorso RobeBase ) : base( 0x13B )
		{
			Type tipo = null;
			
			if( RobeBase != null )
				tipo = RobeBase.GetType();

			TipoBase = tipo;
			SetProps( RobeBase as BaseClothing );
		}

                public TrajeElementos( Serial serial ) : base( serial )
		{
		}
		
 		private void SetProps( BaseClothing RopaBase )
		{
			if( RopaBase != null )
			{
				SAUtility.CopyAosAttributes( RopaBase, this as BaseClothing );
			}

                	Hue = 1357;
                        Name = SAUtility.NombreArtefacto( GetType() );
                        Attributes.BonusMana = 5;
			LootType = LootType.Regular;
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
                        	from.SendMessage("Debes tener equipado el Traje para usarlo.");
                        	return;
                        }

			if( ((dovPlayerMobile)from).TELastDobleClick + delay > DateTime.Now )
			{
                        	from.SendMessage("Debes esperar 20 segundos antes de usar nuevamente el Traje.");
                        	return;
                        }
                        
                        if( from.Mana < 40 )
			{
                        	from.SendMessage("Necesitas 40 de mana para invocar un Elemental.");
                        	return;
                        }

                        if ( (from.Followers + 3) > from.FollowersMax )
			{
                        	from.SendMessage("Tienes muchos seguidores como para invocar otro Elemental.");
                                return;
                        }
                        
                        if ( from.Region is Jail )
                        {
                        	from.SendMessage("No puedes usar el traje aca.");
                        	return;
                        }

			from.RevealingAction();
			InvocarElemental( from );
		}
		
		private void InvocarElemental( Mobile master )
		{
		
			BaseCreature summon = null;

			switch ( Utility.Random( 9 ) )
			{
				case 0:
				{
					summon = new SummonedAirElemental() as BaseCreature;
					break;
				}
				case 1:
				{
					summon = new BloodElemental() as BaseCreature;
					break;
				}
				case 2:
				{
					summon = new Efreet() as BaseCreature;
					break;
				}
				case 3:
				{
					summon = new SummonedFireElemental() as BaseCreature;
					break;
				}
				case 4:
				{
					summon = new IceElemental() as BaseCreature;
					break;
				}
				case 5:
				{
					summon = new ToxicElemental() as BaseCreature;
					break;
				}
				case 6:
				{
					summon = new SummonedWaterElemental() as BaseCreature;
					break;
				}
				case 7:
				{
					summon = new SummonedEarthElemental() as BaseCreature;
					break;
				}
				case 8:
				{
					summon = new SnowElemental() as BaseCreature;
					break;
				}
			}
			
			if( summon != null )
			{
				summon = ModificarSummon( summon );
				summon.Name = "Elemental Summon";
				SpellHelper.Summon( summon, master, 0x217, TimeSpan.FromMinutes( 3 ), false, false );
                        	summon.FixedParticles( 0x3728, 1, 10, 9910, EffectLayer.Head );
                        	master.Mana -= 40;
   			}
   			
   			((dovPlayerMobile)master).TELastDobleClick = DateTime.Now;

		}
		
		private BaseCreature ModificarSummon( BaseCreature summon )
		{
			
			double Mult = 1.2; // props del summon aumentan en 20%

			summon.ControlSlots = 3;
			
			summon.SetStr( (int)(summon.Str*Mult) );
			summon.SetDex( (int)(summon.Dex*Mult) );
			summon.SetInt( (int)(summon.Int*Mult) );

			summon.SetHits( summon.HitsMax );
			summon.SetMana( summon.Int );
			summon.SetStam( summon.Dex );

			summon.SetDamage( (int)(summon.DamageMin*Mult), (int)(summon.DamageMax*Mult) );

			summon.SetDamageType( ResistanceType.Physical, (int)(summon.PhysicalDamage*Mult) );
			summon.SetDamageType( ResistanceType.Fire, (int)(summon.FireDamage*Mult) );
			summon.SetDamageType( ResistanceType.Cold, (int)(summon.ColdDamage*Mult) );
			summon.SetDamageType( ResistanceType.Poison, (int)(summon.PoisonDamage*Mult) );
			summon.SetDamageType( ResistanceType.Energy, (int)(summon.EnergyDamage*Mult) );

                        summon.SetResistance( ResistanceType.Physical, (int)(summon.BasePhysicalResistance*Mult) );
			summon.SetResistance( ResistanceType.Fire, (int)(summon.BaseFireResistance*Mult) );
			summon.SetResistance( ResistanceType.Cold, (int)(summon.BaseColdResistance*Mult) );
			summon.SetResistance( ResistanceType.Poison, (int)(summon.BasePoisonResistance*Mult) );
			summon.SetResistance( ResistanceType.Energy, (int)(summon.BaseEnergyResistance*Mult) );
			
			for( int i=0; i<= summon.Skills.Length-1; i++ )
			{
				if( summon.Skills[i].Base > 0 )
                                	summon.Skills[i].Base *= Mult;
   			}
			
			return summon;

		}

/************************************/
/******* HABILIDAD SECUNDARIA *******/
/************************************/

		public void Switch( Mobile m )
		{
			if ( LastCambioModo + TimeSpan.FromSeconds( SACommands.DelaySwitch ) > DateTime.Now )
			{
                            	m.SendMessage("Debes esperar 3 segundos para cambiar La Resistencia Elegida.");
				return;
			}
			
			if ( !SkillRequerida(m) )
                        {
                        	m.SendMessage( "Necesitas minimo 80 de Spell Resist para cambiar las resistencias del traje." );
                        	return;
                        }
				
			ActivarSwitch( m );
			LastCambioModo = DateTime.Now;
  		}

                public void ActivarSwitch( Mobile m )
                {
                	if ( m == null )
                 		return;

			switch ( ResistAct )
                	{
                		case 0:
                		{
                			ResistAct = 1;
                			break;
                		}
                		case 1:
                		{
                			ResistAct = 2;
                			break;
                		}
                		case 2:
                		{
                			ResistAct = 3;
                			break;
                		}
                		case 3:
                		{
                			ResistAct = 4;
                			break;
                		}
                		case 4:
                		{
                			ResistAct = 5;
                			break;
                		}
                		case 5:
                		{
                			ResistAct = 0;
                			break;
                		}
                	}

                        CambiarRes( m );
                        m.LocalOverheadMessage( MessageType.Regular, 1357, true, "El Traje cambia tus Resistencias!");
			m.FixedParticles( 0x376A, 1, 15, 9961, 1357, 0, EffectLayer.RightHand );
			m.PlaySound( 0x1EF );
			InvalidateProperties();

                }
                
                private void CambiarRes( Mobile m )
                {
                	
                	if ( m == null )
                 		return;

			m.RemoveResistanceMod( rmod1 );
                        m.RemoveResistanceMod( rmod2 );
                        m.RemoveResistanceMod( rmod3 );
                        m.RemoveResistanceMod( rmod4 );
                        m.RemoveResistanceMod( rmod5 );

			switch ( ResistAct )
                	{
				case 0:
                		{
                			rmod1.Offset = 0;
                			rmod2.Offset = 0;
                			rmod3.Offset = 0;
                			rmod4.Offset = 0;
                			rmod5.Offset = 0;
                			break;
                		}
				case 1:
                		{
                			rmod1.Offset = 20;
                			rmod2.Offset = -5;
                			rmod3.Offset = -5;
                			rmod4.Offset = -5;
                			rmod5.Offset = -5;
                			break;
                		}
                		case 2:
                		{
                			rmod1.Offset = -5;
                			rmod2.Offset = 20;
                			rmod3.Offset = -5;
                			rmod4.Offset = -5;
                			rmod5.Offset = -5;
                			break;
                		}
                		case 3:
                		{
                			rmod1.Offset = -5;
                			rmod2.Offset = -5;
                			rmod3.Offset = 20;
                			rmod4.Offset = -5;
                			rmod5.Offset = -5;
                			break;
                		}
                		case 4:
                		{
                			rmod1.Offset = -5;
                			rmod2.Offset = -5;
                			rmod3.Offset = -5;
                			rmod4.Offset = 20;
                			rmod5.Offset = -5;
                			break;
                		}
                		case 5:
                		{
                			rmod1.Offset = -5;
                			rmod2.Offset = -5;
                			rmod3.Offset = -5;
                			rmod4.Offset = -5;
                			rmod5.Offset = 20;
                			break;
                		}
                	}
                	
                	m.AddResistanceMod( rmod1 );
                 	m.AddResistanceMod( rmod2 );
                 	m.AddResistanceMod( rmod3 );
                 	m.AddResistanceMod( rmod4 );
                 	m.AddResistanceMod( rmod5 );
                 	InvalidateProperties();
                	
                }
                
                public bool SkillRequerida( Mobile m )
		{
			if ( m.Skills.MagicResist.Value < 80.0 )
				return false;

			return true;
		}
                
/*****************************************/
/******* OTROS METODOS ADICIONALES *******/
/*****************************************/
		
		private void ActivarDefBonus( Mobile m )
                {
                 	if ( m == null )
                 		return;

                 	m.AddSkillMod( smod1 );

                }

                private void DesactivarBonus( Mobile m )
                {
                        if( m == null )
                        	return;

                        smod1.Remove();
                        m.RemoveResistanceMod( rmod1 );
                        m.RemoveResistanceMod( rmod2 );
                        m.RemoveResistanceMod( rmod3 );
                        m.RemoveResistanceMod( rmod4 );
                        m.RemoveResistanceMod( rmod5 );
                }

                public override bool OnEquip( Mobile from )
                {
			if( SAUtility.MuchosEquipados( from ) )
                        {
                        	from.SendMessage( "No puedes equiparte otro artefacto." );
                        	return false;
                        }

                        ActivarDefBonus( from );
                        
                        if( SkillRequerida( from ) )
				CambiarRes( from );

                	return base.OnEquip( from );
                }
                
                public override void OnRemoved( object parent )
                {
			if ( !(parent is Mobile) )
				return;
			
			Mobile m = (Mobile)parent;
			
                        DesactivarBonus( m );
                        	
                        base.OnRemoved( parent );
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );
			
			String final = "Bonus Magery: " + BonusMagery.ToString() + "\nResistencia Escogida\t";
			
			if( ResistAct == 0 )
                        	final += "Ninguna";
                        else if( ResistAct == 1 )
                                final += "Fisica";
                        else if( ResistAct == 2 )
                                final += "Fuego";
                        else if( ResistAct == 3 )
                                final += "Frio";
                        else if( ResistAct == 4 )
                                final += "Veneno";
                        else if( ResistAct == 5 )
                                final += "Energia";

			list.Add( 1060660, final );

		}


		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( (int) ResistAct );
			writer.Write( m_TipoBase.Name );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

                        ResistAct = reader.ReadInt();
                        TipoBase = ScriptCompiler.FindTypeByName( reader.ReadString() );

			ActivarDefBonus( Parent as Mobile );
			CambiarRes( Parent as Mobile );
		}


	}

}
