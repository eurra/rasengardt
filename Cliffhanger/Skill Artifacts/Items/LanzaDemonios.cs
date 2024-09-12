// Lanza de los Demonios.
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
	public class LanzaDemonios : BaseSpear, SkillArtifact
	{
                private bool m_InvoqueMode = false;

                public static TimeSpan delay = TimeSpan.FromMinutes( 1 );
                private DateTime LastCambioModo = DateTime.Now;
                //private IHumHueTimer huetimer;

                private WeaponAnimation m_WeaponAnimation;
                private WeaponAbility m_PrimaryAbility;
                private WeaponAbility m_SecondaryAbility;
                private int[] BaseSpearValues = new int[10];
                
                public bool InvoqueMode{ get{ return m_InvoqueMode; } }

		public override WeaponAbility PrimaryAbility{ get{ return m_PrimaryAbility; } }
		public override WeaponAbility SecondaryAbility{ get{ return m_SecondaryAbility; } }

		public override int AosStrengthReq{ get{ return BaseSpearValues[0]; } }
		public override int AosMinDamage{ get{ return BaseSpearValues[1]; } }
		public override int AosMaxDamage{ get{ return BaseSpearValues[2]; } }
		public override int AosSpeed{ get{ return BaseSpearValues[3]; } }

		public override int OldStrengthReq{ get{ return BaseSpearValues[4]; } }
		public override int OldMinDamage{ get{ return BaseSpearValues[5]; } }
		public override int OldMaxDamage{ get{ return BaseSpearValues[6]; } }
		public override int OldSpeed{ get{ return BaseSpearValues[7]; } }

		public override int InitMinHits{ get{ return BaseSpearValues[8]; } }
		public override int InitMaxHits{ get{ return BaseSpearValues[9]; } }
		
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
                        		m_TipoBase = typeof(Pike);
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

			BaseSpearValues[0] = weapon.AosStrengthReq;
			BaseSpearValues[1] = weapon.AosMinDamage;
			BaseSpearValues[2] = weapon.AosMaxDamage;
			BaseSpearValues[3] = weapon.AosSpeed;

			BaseSpearValues[4] = weapon.OldStrengthReq;
			BaseSpearValues[5] = weapon.OldMinDamage;
			BaseSpearValues[6] = weapon.OldMaxDamage;
			BaseSpearValues[7] = weapon.OldSpeed;

			BaseSpearValues[8] = weapon.InitMinHits;
			BaseSpearValues[9] = weapon.InitMaxHits;

			Weight = weapon.Weight;
			Layer = weapon.Layer;

			m_WeaponAnimation = weapon.DefAnimation;

			weapon.Delete();
		}

		[Constructable]
		public LanzaDemonios() : this( SAUtility.TipoBaseRandom( typeof(LanzaDemonios) ) )
		{
		}

		[Constructable]
		public LanzaDemonios( Type tipolanza ) : base( 0x13B )
		{
			TipoBase = tipolanza;
			SetProps( null );
  		}

		public LanzaDemonios( BaseSpear LanzaBase ) : base( 0x13B )
		{
			Type tipo = null;
			
			if( LanzaBase != null )
				tipo = LanzaBase.GetType();

			TipoBase = tipo;
			SetProps( LanzaBase as BaseWeapon );
		}

                public LanzaDemonios( Serial serial ) : base( serial )
		{
		}

 		private void SetProps( BaseWeapon ArmaBase )
		{
			if( ArmaBase != null )
                	{
				SAUtility.CopyAosAttributes( ArmaBase, this as BaseWeapon );

                        	//AosAttribute
                        	if( Attributes.WeaponSpeed > 10 )
                        		Attributes.WeaponSpeed = 10;
                        	if( Attributes.WeaponDamage > 15 )
                        		Attributes.WeaponDamage = 15;
                        	if( Attributes.AttackChance > 10 )
                        		Attributes.AttackChance = 10;
                        	if( Attributes.DefendChance > 5 )
                        		Attributes.DefendChance = 5;
                        		
                        	//AosWeaponAttribute
                        	if( WeaponAttributes.UseBestSkill == 1 )
                                	WeaponAttributes.UseBestSkill = 0;
                                if( WeaponAttributes.MageWeapon < 0 )
                                	WeaponAttributes.MageWeapon = 0;
                	}
			
			Hue = 193;
                        Name = SAUtility.NombreArtefacto( GetType() );
                        Attributes.BonusDex = 5;
                        Attributes.BonusHits = 5;
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
                        	from.SendMessage("Debes tener equipada la Lanza para usarla.");
                        	return;
                        }
			
                        if ( !SkillRequerida(from) )
                        {
                        	from.SendMessage("Este movimiento requiere que tengas 100 de Fencing minimo.");
                        	return;
                        }

                        dovPlayerMobile p = (dovPlayerMobile)from;

			if( p.LDLastDobleClick + delay > DateTime.Now )
			{
                        	from.SendMessage("Debes esperar 1 minuto para volver a usar la Lanza.");
                        	return;
                        }
                        
                        if ( from.Region is Jail )
                        {
                        	from.SendMessage("No puedes usar la lanza aca.");
                        	return;
                        }

                        from.SendMessage( "Elige tu objetivo");
			from.Target = new InternalTarget( this );
			from.Target.BeginTimeout( from, TimeSpan.FromSeconds( 30.0 ) );


		}
		
		public bool SkillRequerida( Mobile m )
		{
			if ( m.Skills.Fencing.Value < 100.0 )
				return false;
			
			return true;
		}
		
		private class InternalTarget : Target
		{
			private LanzaDemonios m_Lanza;
			
			public InternalTarget( LanzaDemonios lanza ) :  base( 10, true, TargetFlags.None )
			{
                        	m_Lanza = lanza;
                        }
                        
                        protected override void OnTarget( Mobile from, object o )
                        {
				if ( from.Mana < 10 )
				{
                        		from.SendMessage ("Necesitas 10 puntos de mana para la invocación.");
                                	return;
                        	}
                        
                        	if ( from.Stam < 10 )
				{
                        		from.SendMessage ("Necesitas 10 puntos de estamina para la invocación.");
                                	return;
                        	}

				if ( o is IPoint3D )
					Target( (IPoint3D)o, from );
			}

                        private void Target( IPoint3D p, Mobile from )
                        {
                                if( from == null || !( from is dovPlayerMobile ) )
                        		return;

				Map map = from.Map;

				SpellHelper.GetSurfaceTop( ref p );

				if ( map == null || !from.InLOS( p ) )
				{
					from.SendMessage( "Ese punto es inaccesible." );
					return;
				}

				((dovPlayerMobile)from).LDLastDobleClick = DateTime.Now;
				from.RevealingAction();
				from.Direction = from.GetDirectionTo( p );

				if( !from.Mounted )
					from.Animate( 11, 5, 1, true, false, 0 );

				from.PlaySound( 0x212 );

				Effects.SendLocationEffect( new Point3D( from.X, from.Y, from.Z ), from.Map, 0x3728, 25, 0, 0 );
				object[] args = new object[] { 74 };
				SoulBomber bomb = Activator.CreateInstance( typeof(SoulBomber), args ) as SoulBomber;
				bomb.Name = "Espectro";
				bomb.Hue = 193;
				bomb.MoveToWorld( from.Location, from.Map );
				bomb.ControlOrder = OrderType.Stay;
                                bomb.TargetBomb( new Point2D( p.X, p.Y ) );
				from.Mana -= 10;
				from.Stam -= 10;
                        }
                }

/************************************/
/******* HABILIDAD SECUNDARIA *******/
/************************************/

		public void Switch( Mobile m )
		{
			if ( LastCambioModo + TimeSpan.FromSeconds( SACommands.DelaySwitch ) > DateTime.Now )
			{
                            	m.SendMessage("Debes esperar 3 segundos para cambiar el modo Invocacion.");
				return;
			}
			
			if ( !SkillRequerida(m) )
                        {
                        	m.SendMessage("Activar la lanza requiere que tengas 100 de Fencing minimo.");
                        	return;
                        }

			if ( m_InvoqueMode )
				DesactivarInvoque( m );
                        else
                                ActivarInvoque( m );

  		}

		private void ActivarInvoque ( Mobile m )
		{
                	m.LocalOverheadMessage( MessageType.Regular, 193, true, "La Lanza adquiere fuerzas malignas!");
                	m.PlaySound( 491 );
                	m.FixedParticles( 0x374A, 1, 15, 9961, 193, 0, EffectLayer.LeftHand );

                	//huetimer = new IHumHueTimer( this as Item );
                	//huetimer.Start();

                        m_InvoqueMode = true;
                        InvalidateProperties();
                        LastCambioModo = DateTime.Now;
                }

		private void DesactivarInvoque ( Mobile m )
		{
                	m.LocalOverheadMessage( MessageType.Regular, 193, true, "La Lanza se normaliza.");
			m.PlaySound( 491 );
                	m.FixedParticles( 0x374A, 1, 15, 9961, 193, 0, EffectLayer.LeftHand );

                	//huetimer.Stop();
                	//Hue = 193;
                	
                	m_InvoqueMode = false;
                	InvalidateProperties();
                	LastCambioModo = DateTime.Now;
                }

                private DateTime m_NextSummon;
                
                [CommandProperty( AccessLevel.GameMaster )]
		public DateTime NextSummon{ get{ return m_NextSummon; } set{ m_NextSummon = value; } }

		public override void OnHit( Mobile attacker, Mobile defender )
		{
                        base.OnHit( attacker, defender );
			
			if ( !m_InvoqueMode )
				return;

			if( DateTime.Now < m_NextSummon )
				return;

			while ( attacker.Mana >= 20 && GenerarSummon( attacker ) )
				attacker.Mana -= 20;

		}

		private bool GenerarSummon( Mobile m )
		{
                	BaseCreature summon = null;
                	Type tipo = null;
                	int CS = 0;
                	double factor = 1.0;
                	TimeSpan delay = TimeSpan.FromSeconds( 0.0 );
                	
                	if( m.Karma <= -10000 || m.Karma >= 10000 )
                	{
                		tipo = typeof( Daemon );
				CS = 3;
				factor = 1.3;
				delay = TimeSpan.FromSeconds( 40.0 );
			}
			else if( m.Karma <= -5000 || m.Karma >= 5000 )
			{
                		tipo = typeof( Gargoyle );
                                CS = 2;
                                factor = 1.2;
                                delay = TimeSpan.FromSeconds( 25.0 );
			}
			else
			{
                		tipo = typeof( Imp );
                                CS = 1;
                                delay = TimeSpan.FromSeconds( 20.0 );
			}
			
			if( ( ( m.Followers + CS ) > m.FollowersMax ) || tipo == null )
				return false;

			try
			{
				summon = Activator.CreateInstance( tipo ) as BaseCreature;
			}
			catch
			{
			}

			if( summon != null )
			{
				if( factor != 1.0 )
					PotenciarCriatura( summon, factor );

				summon.Name = "a Spirit";
				summon.ControlSlots = CS;
				summon.Hue = 193;
				
				if( m.Karma >= 0 )
				{
					if( tipo == typeof( Imp ) )
						summon.BodyValue = 128;
					else if( tipo == typeof( Gargoyle ) )
                                                summon.BodyValue = 123;
                                        else
                                                summon.BodyValue = 175;
                                }

				SpellHelper.Summon( summon, m, 0x216, TimeSpan.FromMinutes( 2 ), false, false );
				summon.FixedParticles( 0x3728, 1, 10, 9910, EffectLayer.Head );

				if( m.Combatant != null )
				{
					summon.ControlOrder = OrderType.Attack;
					summon.ControlTarget = m.Combatant;
				}

                                m_NextSummon = DateTime.Now + delay;

				return true;
			}
			
			return false;

		}

		public static void PotenciarCriatura( BaseCreature c, double Mult )
		{
			c.SetStr( (int)(c.Str*Mult) );
			c.SetDex( (int)(c.Dex*Mult) );
			c.SetInt( (int)(c.Int*Mult) );

			c.SetHits( c.HitsMax );
			c.SetMana( c.Int );
			c.SetStam( c.Dex );

			c.SetDamage( (int)(c.DamageMin*Mult), (int)(c.DamageMax*Mult) );

			c.SetDamageType( ResistanceType.Physical, (int)(c.PhysicalDamage*Mult) );
			c.SetDamageType( ResistanceType.Fire, (int)(c.FireDamage*Mult) );
			c.SetDamageType( ResistanceType.Cold, (int)(c.ColdDamage*Mult) );
			c.SetDamageType( ResistanceType.Poison, (int)(c.PoisonDamage*Mult) );
			c.SetDamageType( ResistanceType.Energy, (int)(c.EnergyDamage*Mult) );

                        c.SetResistance( ResistanceType.Physical, (int)(c.BasePhysicalResistance*Mult) );
			c.SetResistance( ResistanceType.Fire, (int)(c.BaseFireResistance*Mult) );
			c.SetResistance( ResistanceType.Cold, (int)(c.BaseColdResistance*Mult) );
			c.SetResistance( ResistanceType.Poison, (int)(c.BasePoisonResistance*Mult) );
			c.SetResistance( ResistanceType.Energy, (int)(c.BaseEnergyResistance*Mult) );
			
			for( int i=0; i<= c.Skills.Length-1; i++ )
			{
				if( c.Skills[i].Base > 0 )
                                	c.Skills[i].Base *= Mult;
   			}
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
				cold = nrgy = 0;
				fire = 50;
				phys = 25;
				pois = 25;
			}
		}
                
                public override bool OnEquip( Mobile from )
                {
			if( SAUtility.MuchosEquipados( from ) )
                        {
                        	from.SendMessage( "No puedes equiparte otro artefacto." );
                        	return false;
                        }
                        
                        if ( m_InvoqueMode )
                	{
                       		if( !SkillRequerida( from ) )
                                	DesactivarInvoque( from );
                         	else
					ActivarInvoque( from );

                        	LastCambioModo = DateTime.Now;
                        }

                	return base.OnEquip( from );

                }
                
                public override void OnRemoved( object parent )
                {
			if ( !(parent is Mobile) )
				return;

                        if ( m_InvoqueMode )
                	{
                        	DesactivarInvoque( (Mobile)parent );
                                m_InvoqueMode = true;
                        }
                        
                        base.OnRemoved( parent );

		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );
			list.Add( 1060660, "Modo Invocacion\t" + ( m_InvoqueMode? "Activado" : "Desactivado" ) );
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
