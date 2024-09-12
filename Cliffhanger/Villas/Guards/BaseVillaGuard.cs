// Guardia de Villa Base.
// Hecho por Cliffhanger (archon.cl@gmail.com)

using System;
using System.Collections;
using Server.Misc;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;
using Server.Regions;

namespace Server.Mobiles
{
	public abstract class BaseVillaGuard : BaseCreature
	{
		private Timer m_IdleTimer;

		private bool m_esConst;

		private Mobile m_Focus;
		private Point3D FocusStartPoint;
		
		public override bool IsScaredOfScaryThings{ get{ return false; } }
		public override bool BardImmune{ get{ return true; } }
		public override bool AutoDispel{ get{ return true; } }
		public override bool DeleteCorpseOnDeath{ get{ return true; } }

		[Constructable]
		public BaseVillaGuard( AIType ai, FightMode mode, int iRangePerception, int iRangeFight, double dActiveSpeed, double dPassiveSpeed,  Mobile target ) : base( ai, mode, iRangePerception, iRangeFight, dActiveSpeed, dPassiveSpeed )
		{
			if ( target != null )
			{
				Location = target.Location;
				Map = target.Map;

				this.PlaySound( 0x1FE );
				Effects.SendLocationParticles( EffectItem.Create( Location, Map, EffectItem.DefaultDuration ), 0x3728, 10, 10, 5023 );
			}
			else
                        	m_esConst = true;
                        	
                        SetStr( 90, 110 );
			SetDex( 90, 110 );
			SetInt( 40, 60 );
			
			SetHits( 100, 110 );

			SetDamage( 15, 18 );
			
			SetDamageType( ResistanceType.Physical, 100 );
		}

		public BaseVillaGuard( Serial serial ) : base( serial )
		{
		}
		
		public void AddHairAndBeard( bool forceAdd )
		{
			int hairHue = Utility.RandomHairHue();

			if( Utility.RandomBool() || forceAdd )
				AddHair( hairHue );

			if( Utility.RandomBool() || forceAdd )
			{
				Item beard = new Item( Utility.RandomList( 0x203E, 0x203F, 0x2040, 0x2041, 0x204B, 0x204C, 0x204D ) );

				beard.Hue = hairHue;
				beard.Layer = Layer.FacialHair;
				beard.Movable = false;

				AddItem( beard );
			}
		}
		
		public void AddHair( int hue )
		{
                        Item hair = Server.Items.Hair.GetRandomHair( false, hue );
                        hair.Movable = false;

			AddItem( hair );
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool esConst
		{
		       	get{ return m_esConst; }
			
			set
			{
				if( m_esConst == value )
					return;

				m_esConst = value;

				if( !m_esConst )
				{
					if( m_IdleTimer != null )
					{
						m_IdleTimer.Stop();
						m_IdleTimer = null;
					}

					m_IdleTimer = new IdleTimer( this );
					m_IdleTimer.Start();
				}
			}
			
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Focus
		{
			get{ return m_Focus; }

			set
			{
				if ( Deleted )
					return;
					
				if( value != null && !CanBeHarmful( value ) )
					return;
					
				Mobile oldFocus = m_Focus;

				if ( oldFocus != value )
				{
					m_Focus = value;

					Combatant = value;
					
					if ( oldFocus != null )
					{
						if( !oldFocus.Alive )
						{
							if ( oldFocus.Player )
                                                        	Say( KillMessage( true ) );
							else if ( oldFocus is BaseCreature )
                                                        	Say( KillMessage( false ) );
                                                        	
                                                        if( Region is BaseVilla )
                                                		((BaseVilla)Region).RemoveCandidate( oldFocus );
      						}
                                                
						RemoveAggressor( oldFocus );
						RemoveAggressed( oldFocus );
     					}

                                        if ( m_Focus != null )
                                        {
						if ( m_Focus.Player )
                                                        Say( AttackMessage( true ) );
						else if ( m_Focus is BaseCreature )
                                                        Say( AttackMessage( false ) );

						if ( m_IdleTimer != null && !m_esConst )
						{
							m_IdleTimer.Stop();
							m_IdleTimer = null;
						}

						AggressiveAction( value );
						FocusStartPoint = Location;
					}
					else
					{
						if( !m_esConst )
						{
							if ( m_IdleTimer != null )
                                                        {
								m_IdleTimer.Stop();
								m_IdleTimer = null;
							}

							m_IdleTimer = new IdleTimer( this );
							m_IdleTimer.Start();
						}
					}
				}
				else if ( m_Focus == null && m_IdleTimer == null && !m_esConst )
				{
					m_IdleTimer = new IdleTimer( this );
					m_IdleTimer.Start();
				}

			}
		}
		
		private string AttackMessage( bool esPlayer )
		{
			switch( Utility.Random( 4 ) )
			{
				case 0: return ( esPlayer ? "Pagaras tu osadia!" : "Pagaras tu osadia, criatura!" );
				case 1: return ( esPlayer ? "Te arrepentiras de andar por estos lugares!" : "Te arrepentiras de andar por estos lugares, engendro!" );
                                case 2: return ( esPlayer ? "No tendre compasion contigo, debilucho!" : "No tendre compasion contigo, maldita criatura!" );
                                default: return ( esPlayer ? "Te dare una leccion, maldito!" : "Te dare una leccion, maldita criatura!" );
   			}
   		}
   		
   		private string KillMessage( bool esPlayer )
		{
			switch( Utility.Random( 4 ) )
			{
				case 0: return ( esPlayer ? "Eso te enseñara a respetar las reglas!" : "Eso te enseñara a respetar las reglas criatura!" );
				case 1: return ( esPlayer ? "No hay espacio para ti en un lugar como este!" : "No hay espacio para una criatura como tu en un lugar como este!" );
                                case 2: return ( esPlayer ? "Y para la otra te ira peor!" : "Y para la otra te ira peor criatura!" );
                                default: return ( esPlayer ? "Espero hayas aprendido la leccion!" : "Espero hayas aprendido la leccion, maldita criatura!" );
   			}
   		}

		public override void OnAfterDelete()
		{

			if ( m_IdleTimer != null )
			{
				m_IdleTimer.Stop();
				m_IdleTimer = null;
			}

			base.OnAfterDelete();
		}

		public override void OnThink()
		{
			base.OnThink();
			
			if( !( Region is BaseVilla ) || ((BaseVilla)Region).GuardType != GetType() )
			{
				Effects.SendLocationParticles( EffectItem.Create( Location, Map, EffectItem.DefaultDuration ), 0x3728, 10, 10, 2023 );
				PlaySound( 0x1FE );
				Delete();
			}
			
			if( m_Focus != null )
			{
				Criminal = false;
				Kills = 0;
				Stam = StamMax;

				if ( m_Focus.Deleted || !m_Focus.Alive || m_Focus.Region != Region || !CanBeHarmful( m_Focus ) )
				{
					if( m_Focus.Region != Region )
						TeleportToPos( FocusStartPoint );

					Focus = null;
					return;
				}
				else if ( Weapon is Fists )
				{
					Kill();
					return;
				}
				
				if ( m_Focus != null && Combatant != m_Focus )
					Combatant = m_Focus;

				AIFocusActive();
			}
			else
			{
				if ( Combatant != null && ( !InRange( Combatant, 20 ) || Region != Combatant.Region ) )
				{
					Combatant = null;
					Warmode = false;
					return;
				}
				
				BaseVilla reg = Region as BaseVilla;
				
				if( reg == null )
					return;
				
				for( int i=0; i < reg.GuardCandidates.Count; i++ )
				{
					Mobile m = (Mobile)reg.GuardCandidates[i];

					if( InRange( m, 8 ) && InLOS( m ) && m.Alive && Region == m.Region )
					{
						Focus = m;
						break;
					}
				}
   			}
		}

		public virtual void AIFocusActive()
		{
			if ( !InRange( m_Focus, 20 ) )
			{
				Focus = null;
			}
			else if ( !InRange( m_Focus, 10 ) || !InLOS( m_Focus ) )
			{
				TeleportTo( m_Focus );
			}
			else if ( !CanSee( m_Focus ) )
			{
				if ( Utility.Random( 50 ) == 0 )
				{
					Say( "Aparece!" );
                                        m_Focus.RevealingAction();
    				}
			}
		}

		public void TeleportTo( Mobile target )
		{
			Point3D from = Location;
			Point3D to = target.Location;

			Location = to;

			Effects.SendLocationParticles( EffectItem.Create( from, Map, EffectItem.DefaultDuration ), 0x3728, 10, 10, 2023 );
			Effects.SendLocationParticles( EffectItem.Create(   to, Map, EffectItem.DefaultDuration ), 0x3728, 10, 10, 5023 );

			PlaySound( 0x1FE );
		}
		
		private void TeleportToPos( Point3D to )
		{
			Point3D from = Location;

			Location = to;

			Effects.SendLocationParticles( EffectItem.Create( from, Map, EffectItem.DefaultDuration ), 0x3728, 10, 10, 2023 );
			Effects.SendLocationParticles( EffectItem.Create(   to, Map, EffectItem.DefaultDuration ), 0x3728, 10, 10, 5023 );

			PlaySound( 0x1FE );
		}

		public override bool OnBeforeDeath()
		{
			if ( m_Focus != null && m_Focus.Alive )
			{
				new AvengeTimer( m_Focus ).Start(); // If a guard dies, two more guards will spawn
				Say("La venganza sera terribleeaeeaaaAAarg!");
			}

			return base.OnBeforeDeath();
		}
		
		public override void AddNameProperties( ObjectPropertyList list )
		{
    			string name = Name;

			if ( name == null )
				name = String.Empty;
			
			string suffix = "";

			if ( ClickTitle && Title != null && Title.Length > 0 )
				suffix = Title;

			string prefix = "";
				
			list.Add( 1050045, "{0} \t{1}\t{2}", prefix, name, suffix ); // ~1_PREFIX~~2_NAME~~3_SUFFIX~
		}


		private class AvengeTimer : Timer
		{
			private Mobile m_Focus;

			public AvengeTimer( Mobile focus ) : base( TimeSpan.FromSeconds( 2.5 ), TimeSpan.FromSeconds( 1.0 ), 2 )
			{
				m_Focus = focus;
			}

			protected override void OnTick()
			{
				if( !m_Focus.Alive )
				{
					Stop();
					return;
				}

				if( m_Focus.Region is BaseVilla )
				{
					((BaseVilla)m_Focus.Region).SpawnGuard( m_Focus );
					return;
				}
				else
					Stop();
			}
		}

		private class IdleTimer : Timer
		{
			private BaseVillaGuard m_Owner;

			public IdleTimer( BaseVillaGuard owner ) : base( TimeSpan.FromSeconds( 15.0 ) )
			{
				m_Owner = owner;
			}

			protected override void OnTick()
			{
				if ( m_Owner.Deleted )
				{
     					Stop();
					return;
				}

				Effects.SendLocationParticles( EffectItem.Create( m_Owner.Location, m_Owner.Map, EffectItem.DefaultDuration ), 0x3728, 10, 10, 2023 );
				m_Owner.PlaySound( 0x1FE );

				m_Owner.Delete();

			}
		}
		
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( m_Focus );
			writer.Write( m_esConst );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 0:
				{
					m_Focus = reader.ReadMobile();
					m_esConst = reader.ReadBool();

					if( m_Focus == null && !m_esConst )
					{
						m_IdleTimer = new IdleTimer( this );
						m_IdleTimer.Start();
					}

					break;
				}
			}
		}
	}
}
