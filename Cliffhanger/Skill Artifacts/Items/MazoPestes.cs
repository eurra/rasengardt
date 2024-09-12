// Mazo de las Pestes.
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
	public class MazoPestes : BaseBashing, SkillArtifact
	{
                private bool m_PoisonDefMode = false;
                private bool m_PoisonAttackMode = false;
                private static double chancedef = 0.3; // Chance de que la defensa venenosa actue.

                public static TimeSpan delay = TimeSpan.FromSeconds( 10.0 );
                private DateTime LastCambioModo = DateTime.Now;
                //private IEnaHueTimer huetimer;

                private WeaponAbility m_PrimaryAbility;
                private WeaponAbility m_SecondaryAbility;
                private WeaponAnimation m_WeaponAnimation;
                private int[] BaseMaceValues = new int[10];

                public bool PoisonDefMode{ get{ return m_PoisonDefMode; } }
                public bool PoisonAttackMode{ get{ return m_PoisonAttackMode; } }

		public override WeaponAbility PrimaryAbility{ get{ return m_PrimaryAbility; } }
		public override WeaponAbility SecondaryAbility{ get{ return m_SecondaryAbility; } }

		public override int AosStrengthReq{ get{ return BaseMaceValues[0]; } }
		public override int AosMinDamage{ get{ return BaseMaceValues[1]; } }
		public override int AosMaxDamage{ get{ return BaseMaceValues[2]; } }
		public override int AosSpeed{ get{ return BaseMaceValues[3]; } }

		public override int OldStrengthReq{ get{ return BaseMaceValues[4]; } }
		public override int OldMinDamage{ get{ return BaseMaceValues[5]; } }
		public override int OldMaxDamage{ get{ return BaseMaceValues[6]; } }
		public override int OldSpeed{ get{ return BaseMaceValues[7]; } }

		public override int InitMinHits{ get{ return BaseMaceValues[8]; } }
		public override int InitMaxHits{ get{ return BaseMaceValues[9]; } }
		
		public override WeaponAnimation DefAnimation{ get{ return m_WeaponAnimation; } }

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
                        		m_TipoBase = typeof(Mace);
				else
                        		m_TipoBase = value;

				ConfigurarTipoBase();
				InvalidateProperties();
			}
		}

		private void ConfigurarTipoBase()
		{
			BaseWeapon weapon = Activator.CreateInstance( m_TipoBase ) as BaseWeapon;

			if( weapon == null )
				return;

  			ItemID = weapon.ItemID;

			m_PrimaryAbility = weapon.PrimaryAbility;
			m_SecondaryAbility = weapon.SecondaryAbility;

			BaseMaceValues[0] = weapon.AosStrengthReq;
			BaseMaceValues[1] = weapon.AosMinDamage;
			BaseMaceValues[2] = weapon.AosMaxDamage;
			BaseMaceValues[3] = weapon.AosSpeed;

			BaseMaceValues[4] = weapon.OldStrengthReq;
			BaseMaceValues[5] = weapon.OldMinDamage;
			BaseMaceValues[6] = weapon.OldMaxDamage;
			BaseMaceValues[7] = weapon.OldSpeed;

			BaseMaceValues[8] = weapon.InitMinHits;
			BaseMaceValues[9] = weapon.InitMaxHits;

			m_WeaponAnimation = weapon.DefAnimation;

			Weight = weapon.Weight;
			Layer = weapon.Layer;

			weapon.Delete();
		}
		
		[Constructable]
		public MazoPestes() : this( SAUtility.TipoBaseRandom( typeof(MazoPestes) ) )
		{
		}

		[Constructable]
		public MazoPestes( Type tipomazo ) : base( 0x13B )
		{
			TipoBase = tipomazo;
			SetProps( null );
  		}
  		
		public MazoPestes( BaseWeapon MazoBase ) : base( 0x13B )
		{
			Type tipo = null;
			
			if( MazoBase != null )
				tipo = MazoBase.GetType();

			TipoBase = tipo;
			SetProps( MazoBase );
		}

                public MazoPestes( Serial serial ) : base( serial )
		{
		}

		public void SetProps( BaseWeapon ArmaBase )
		{
			if( ArmaBase != null )
                	{
				SAUtility.CopyAosAttributes( ArmaBase, this as BaseWeapon );

                        	//AosAttribute
                        	if( Attributes.WeaponSpeed > 10 )
                        		Attributes.WeaponSpeed = 10;
                        	if( Attributes.WeaponDamage > 30 )
                        		Attributes.WeaponDamage = 30;
                        	if( Attributes.AttackChance > 5 )
                        		Attributes.AttackChance = 5;
                        	if( Attributes.DefendChance > 5 )
                        		Attributes.DefendChance = 5;

                        	//AosWeaponAttribute
                        	if( WeaponAttributes.UseBestSkill == 1 )
                                	WeaponAttributes.UseBestSkill = 0;
                                if( WeaponAttributes.MageWeapon < 0 )
                                	WeaponAttributes.MageWeapon = 0;
                	}
                	
                	Hue = 2111;
                        Name = SAUtility.NombreArtefacto( GetType() );
                        Attributes.BonusMana = 10;
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
                        	from.SendMessage("Debes tener equipado el Mazo para usarlo.");
                        	return;
                        }
                        
                        if( PrimaryAbility == WeaponAbility.BleedAttack || SecondaryAbility == WeaponAbility.BleedAttack )
                        {
                        	from.SendMessage("No puedes usar esta habilidad con este tipo de Mazo.");
                        	return;
                        }

                        if ( !SkillRequerida(from) )
                        {
                        	from.SendMessage("Este movimiento requiere que tengas 100 de MaceFighting minimo.");
                        	return;
                        }

			if ( m_PoisonDefMode )
			{
                                from.SendMessage("No puedes hacer este movimiento con el modo Venenoso Activado!.");
                        	return;
                        }

			if( ((dovPlayerMobile)from).MPLastDobleClick + delay > DateTime.Now )
			{
                        	from.SendMessage("Debes esperar 20 segundos para volver a usar el Mazo.");
                        	return;
                        }
                        
                        if ( m_PoisonAttackMode )
			{
                                from.SendMessage("El Mazo ya esta cargado con veneno!");
                        	return;
                        }

                        Container pack = from.Backpack;

			if ( !pack.ConsumeTotal( typeof(NoxCrystal), 5 ) )
			{
				from.SendMessage("No tienes suficientes Nox Crystals en tu Bolso.");
				return;
			}

                        from.RevealingAction();
			InfectarMazo( from );

		}

		private void InfectarMazo( Mobile from )
		{
			((dovPlayerMobile)from).MPLastDobleClick = DateTime.Now;

			m_PoisonAttackMode = true;

                        from.LocalOverheadMessage( MessageType.Regular, 2111, true, "El Mazo se impregna de pestes!");
                	from.PlaySound( 0xDD );
                	from.FixedParticles( 0x3728, 1, 15, 9961, 2111, 0, EffectLayer.RightHand );
  		}
  		
		public override void OnHit( Mobile attacker, Mobile defender )
		{
                        base.OnHit( attacker, defender );

			if ( !m_PoisonAttackMode )
				return;

                        defender.ApplyPoison( attacker, Poison.Greater );
                        defender.FixedParticles( 0x36B0, 1, 15, 9961, 2111, 0, EffectLayer.RightHand );
                        defender.PlaySound( 0x229 );
                        m_PoisonAttackMode = false;
		}
		
		public override void OnMiss( Mobile attacker, Mobile defender )
		{
                        base.OnMiss( attacker, defender );

			if ( !m_PoisonAttackMode )
				return;

                        attacker.ApplyPoison( defender, Poison.Greater );
			attacker.FixedParticles( 0x36B0, 1, 15, 9961, 2111, 0, EffectLayer.RightHand );
                        attacker.PlaySound( 0x229 );
                        m_PoisonAttackMode = false;
		}

		public bool SkillRequerida( Mobile m )
		{
			if ( m.Skills.Macing.Value < 100.0 )
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
                            	m.SendMessage("Debes esperar 3 segundos para cambiar el modo Venenoso.");
				return;
			}
			
			if ( !SkillRequerida(m) )
                        {
                        	m.SendMessage("Este modo requiere que tengas 100 de MaceFighting minimo.");
                        	return;
                        }
				
			if ( m_PoisonDefMode )
				DesactivarPoisonDef( m );
                        else
                                ActivarPoisonDef( m );
  		}

		public void ActivarPoisonDef ( Mobile m )
		{
                	m.LocalOverheadMessage( MessageType.Regular, 2111, true, "El Mazo emite pestes!");
                	m.PlaySound( 0x229 );
                	m.FixedParticles( 0x36B0, 1, 15, 9961, 2111, 0, EffectLayer.RightHand );

                	//huetimer = new IEnaHueTimer( this as Item );
                	//huetimer.Start();

                	m_PoisonAttackMode = false;
                        m_PoisonDefMode = true;
                       	InvalidateProperties();
                        LastCambioModo = DateTime.Now;
                }

		public void DesactivarPoisonDef ( Mobile m )
		{
                        m.LocalOverheadMessage( MessageType.Regular, 2111, true, "El Mazo se normaliza.");
			m.PlaySound( 0x229 );
                	m.FixedParticles( 0x36B0, 1, 15, 9961, 2111, 0, EffectLayer.RightHand );

                	//huetimer.Stop();
                	//Hue = 2111;
                	
                	m_PoisonDefMode = false;
                        InvalidateProperties();
                        LastCambioModo = DateTime.Now;
                }
                
                public void InfectarDef( Mobile attacker, Mobile defender )
                {
			if ( chancedef < Utility.RandomDouble() )
				return;

			if ( defender.Stam < 10 )
				return;

			Container pack = defender.Backpack;

			if ( !pack.ConsumeTotal( typeof(NoxCrystal), 10 ) )
				return;

			defender.Stam -= 10;

			attacker.FixedParticles( 0x36B0, 1, 15, 9961, 2111, 0, EffectLayer.RightHand );
			attacker.SendMessage("Te has envenenado con el Mazo Enano!");
                        attacker.PlaySound( 0x229 );
			attacker.ApplyPoison( defender, Poison.Greater );
                }

/*****************************************/
/******* OTROS METODOS ADICIONALES *******/
/*****************************************/

               	public override void GetDamageTypes( Mobile wielder, out int phys, out int fire, out int cold, out int pois, out int nrgy )
		{
			if( PrimaryAbility == WeaponAbility.ArmorIgnore || SecondaryAbility == WeaponAbility.ArmorIgnore )
			{
                        	pois = nrgy = cold = fire = 0;
				phys = 100;
			}
			else
			{
				cold = nrgy = fire = 0;
				phys = 25;
				pois = 75;
			}
		}

                public override bool OnEquip( Mobile from )
                {
			if( SAUtility.MuchosEquipados( from ) )
                        {
                        	from.SendMessage( "No puedes equiparte otro artefacto." );
                        	return false;
                        }
                        	
                        if ( m_PoisonDefMode )
                	{
                       		if( !SkillRequerida( from ) )
                                	DesactivarPoisonDef( from );
                         	else
					ActivarPoisonDef( from );

                        	LastCambioModo = DateTime.Now;
                        }

                	return base.OnEquip( from );
                }

                public override void OnRemoved( object parent )
                {
			if ( !(parent is Mobile) )
				return;

                        if ( m_PoisonDefMode )
                        {
                        	DesactivarPoisonDef( (Mobile)parent );
                        	m_PoisonDefMode = true;
                        }

                        base.OnRemoved( parent );

		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );
			list.Add( 1060660, "Modo Venenoso\t" + ( m_PoisonDefMode ? "Activado" : "Desactivado" ) );
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

			this.Hue = 2111;
		}

	}

}
