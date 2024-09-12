using System;
using System.Collections;
using Server;
using Server.Spells;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles 
{
	[CorpseName( "an fire master corpse" )]
	public class FireMaster : BaseCreature 
	{ 
		[Constructable] 
		public FireMaster() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{ 
			Name = NameList.RandomName( "evil mage" );
			Title = "The Fire Master";
			Body = 124;
			Hue = 2844;

			SetStr( 81, 105 );
			SetDex( 91, 115 );
			SetInt( 520, 590 );

			SetHits( 500, 650 );

			SetDamage( 5, 10 );

			SetDamageType( ResistanceType.Physical, 50 );
			SetDamageType( ResistanceType.Fire, 50 );

			SetResistance( ResistanceType.Physical, 50, 65 );
			SetResistance( ResistanceType.Fire, 75, 85 );
			SetResistance( ResistanceType.Fire, 60, 65 );
			SetResistance( ResistanceType.Poison, 40, 50 );
			SetResistance( ResistanceType.Energy, 55, 65 );

			SetSkill( SkillName.EvalInt, 105.1, 125.0 );
			SetSkill( SkillName.Magery, 115.1, 120.0 );
			SetSkill( SkillName.MagicResist, 100.0, 105.5 );
			SetSkill( SkillName.Tactics, 75.0, 90.5 );
			SetSkill( SkillName.Wrestling, 95.2, 110.0 );

			Fame = 5000;
			Karma = -5000;

			VirtualArmor = 16;

			PackReg( 6 );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich );
			AddLoot( LootPack.MedScrolls );
		}

		public override bool CanRummageCorpses{ get{ return true; } }
		public override bool AlwaysMurderer{ get{ return true; } }
		public override int TreasureMapLevel{ get{ return Core.AOS ? 1 : 0; } }

		public FireMaster( Serial serial ) : base( serial )
		{
		}
		
		private DateTime m_NextSpecialAttack;
		FireWallTimer m_Timer1;
		FireSwarmTimer m_Timer2;

		public override void OnThink()
		{
			base.OnThink();
			
			if( Combatant != null && DateTime.Now > m_NextSpecialAttack )
			{
				if( !InRange( Combatant, 5 ) )
				{
					DoFireWallAttack();
				}
				else
				{
					double rand = Utility.RandomDouble();

					if( ( Hits <= HitsMax/5 ) || rand < 0.1 )
						DoDemonSpawnAttack();
					else
						DoFireSwarmAttack();
				}
			}
		}

		private void DoFireWallAttack()
		{
			if( ( m_Timer1 != null && m_Timer1.Running ) || Combatant == null || Map != Combatant.Map || InRange( Combatant, 5 ) )
				return;

			SpellHelper.Turn( this, Combatant );

			if( m_Timer1 != null )
				m_Timer1 = null;

			m_Timer1 = new FireWallTimer( Location, Combatant.Location, this );
			Say( Location + " " + Combatant.Location );
			m_Timer1.Start();

                        m_NextSpecialAttack = DateTime.Now + TimeSpan.FromSeconds( Utility.RandomMinMax( 6, 12 ) );
		}

		private void DoFireSwarmAttack()
		{
			if( ( m_Timer2 != null && m_Timer2.Running ) || Combatant == null || Map != Combatant.Map )
				return;
				
			SpellHelper.Turn( this, Combatant );

			if( m_Timer2 != null )
				m_Timer2 = null;

			m_Timer2 = new FireSwarmTimer( this );
                        m_Timer2.Start();

                        m_NextSpecialAttack = DateTime.Now + TimeSpan.FromSeconds( Utility.RandomMinMax( 15, 20 ) );
		}
		
		private void DoDemonSpawnAttack()
		{
			if( Combatant == null || Map != Combatant.Map )
				return;

			int eggs = Utility.RandomMinMax( 4, 6 );
			int count = 0;

			for( int i = 0; i < eggs ; i++ )
			{
				int x = Utility.RandomMinMax( Location.X - 5, Location.X + 5 );
                                int y = Utility.RandomMinMax( Location.Y - 5, Location.Y + 5 );
                                int z = Map.GetAverageZ( x, y );
                                
                                count++;

                                if( Map.CanFit( x, y, z, 16, false, true ) )
                                {
                                	Effects.PlaySound( new Point3D( x, y, z ), Map, 0x307 );
                                	
                                	DemonEgg egg = new DemonEgg();
					egg.MoveToWorld( new Point3D( x, y, z ), Map );

					Effects.SendBoltEffect( egg, true, 0 );
					Effects.SendLocationEffect( new Point3D( x, y, z ), Map, 0x36B0, 15, 0, 0 );
					MovingEffect( egg, 0x36D4, 9, 1, false, true, 0, 0 );
                                }
                                else if( count < 100 )
                                {
                                	i--;
                                }
                        }

                        m_NextSpecialAttack = DateTime.Now + TimeSpan.FromSeconds( Utility.RandomMinMax( 25, 35 ) );
		}

		public override void AlterSpellDamageFrom( Mobile from, ref int damage )
		{
			if( m_Timer2 != null && m_Timer2.Running )
				damage = 0;
			else
				base.AlterSpellDamageFrom( from, ref damage );
		}
		
		public override void AlterMeleeDamageFrom( Mobile from, ref int damage )
		{
			if( m_Timer2 != null && m_Timer2.Running )
				damage = 0;
			else
				base.AlterMeleeDamageFrom( from, ref damage );
		}
		
		private class FireWallTimer : Timer
		{
			private Point3D m_CurrentLoc, m_DestLoc;
			private Mobile m_Caster;

			public FireWallTimer( Point3D currloc, Point3D destloc, Mobile caster ) : base( TimeSpan.FromSeconds( 0.5 ), TimeSpan.FromSeconds( 0.1 ) )
			{
                        	m_CurrentLoc = currloc;
                                m_DestLoc = destloc;
                                m_Caster = caster;
                                m_Caster.Frozen = true;

                                m_Caster.BoltEffect( 2844 );
                                Effects.SendLocationEffect( m_Caster.Location, m_Caster.Map, 0x36B0, 15, 2844, 0 );
                                Effects.PlaySound( m_Caster.Location, m_Caster.Map, 0x307 );
                        }
                        
                        protected override void OnTick()
                        {
                        	if( m_Caster == null || !m_Caster.Alive )
                                {
                        		Stop();
                        		return;
                        	}

				Point3D next = ComputeNextPoint( m_CurrentLoc, m_DestLoc );
                        	
                        	if( next == Point3D.Zero )
                        	{
                        		Stop();
                        		return;
                        	}

				IPoint3D inext = (IPoint3D)next;

				SpellHelper.GetSurfaceTop( ref inext );
				
				if( !m_Caster.Map.CanFit( next.X, next.Y, next.Z, 16, false, true ) )
                        	{
                        		Stop();
                        		m_Caster.Frozen = false;
                        		return;
                        	}
				
				ArrayList arr = new ArrayList();

				foreach( Mobile m in m_Caster.Map.GetMobilesInRange( next, 3 ) )
				{
					if( m != null && m.Alive && ( m_Caster is BaseCreature ? ((BaseCreature)m_Caster).IsEnemy( m ) : true ) )
					{
						arr.Add( m );
                                        }
                        	}
                        	
                        	foreach( Mobile m in arr )
                        	{
                        		if( m.InRange( next, 1 ) )
                                		SpellHelper.Damage( TimeSpan.Zero, m, m_Caster, Utility.RandomMinMax( 35, 45 ), 0, 100, 0, 0, 0 );
					else if( m.InRange( next, 2 ) )
                                        	SpellHelper.Damage( TimeSpan.Zero, m, m_Caster, Utility.RandomMinMax( 10, 15 ), 0, 100, 0, 0, 0 );
	                                else if( m.InRange( next, 3 ) )
                                        	SpellHelper.Damage( TimeSpan.Zero, m, m_Caster, Utility.RandomMinMax( 3, 6 ), 0, 100, 0, 0, 0 );
                                }
                        	
                        	Effects.PlaySound( next, m_Caster.Map, 0x208 );

                        	Effects.SendLocationEffect( next, m_Caster.Map, 0x3709, 15, 2844, 0 );
				Effects.SendLocationEffect( new Point3D( next.X - 1, next.Y, next.Z ), m_Caster.Map, 0x3709, 5, 0, 0 );
            			Effects.SendLocationEffect( new Point3D( next.X + 1, next.Y, next.Z ), m_Caster.Map, 0x3709, 5, 0, 0 );
            			Effects.SendLocationEffect( new Point3D( next.X, next.Y - 1, next.Z ), m_Caster.Map, 0x3709, 5, 0, 0 );
            			Effects.SendLocationEffect( new Point3D( next.X, next.Y + 1, next.Z ), m_Caster.Map, 0x3709, 5, 0, 0 );
                        	
                                m_CurrentLoc = next;
                                
                                if( m_CurrentLoc == m_DestLoc )
                                {
                        		Stop();
                        		m_Caster.Frozen = false;
                        		return;
                        	}
                        }

                        private Point3D ComputeNextPoint( Point3D curr, Point3D dest )
                        {
                        	if( curr == Point3D.Zero || dest == Point3D.Zero )
                        		return Point3D.Zero;
                        		
                        	int dx = curr.X - dest.X;
				int dy = curr.Y - dest.Y;

				if( dx > 0 && dy > 0 )
				{
					if( Math.Abs( dx ) >= Math.Abs ( dy ) )
						return new Point3D( curr.X - 1, curr.Y, curr.Z );
					else
                                                return new Point3D( curr.X, curr.Y - 1, curr.Z );
                                }
                                else if( dx < 0 && dy > 0 )
                                {
					if( Math.Abs( dx ) >= Math.Abs ( dy ) )
						return new Point3D( curr.X + 1, curr.Y, curr.Z );
					else
                                                return new Point3D( curr.X, curr.Y - 1, curr.Z );
                                }
                                else if( dx < 0 && dy < 0 )
                                {
					if( Math.Abs( dx ) >= Math.Abs ( dy ) )
						return new Point3D( curr.X + 1, curr.Y, curr.Z );
					else
                                                return new Point3D( curr.X, curr.Y + 1, curr.Z );
                                }
                                else if( dx > 0 && dy < 0 )
                                {
					if( Math.Abs( dx ) >= Math.Abs ( dy ) )
						return new Point3D( curr.X - 1, curr.Y, curr.Z );
					else
                                                return new Point3D( curr.X, curr.Y + 1, curr.Z );
                                }
                                else if( dx == 0 && dy < 0 )
                                {
					return new Point3D( curr.X, curr.Y + 1, curr.Z );
                                }
                                else if( dx == 0 && dy > 0 )
                                {
					return new Point3D( curr.X, curr.Y - 1, curr.Z );
                                }
                                else if( dx < 0 && dy == 0 )
                                {
					return new Point3D( curr.X + 1, curr.Y, curr.Z );
                                }
                                else
                                {
					return new Point3D( curr.X - 1, curr.Y, curr.Z );
                                }
                        }
                }

                private class FireSwarmTimer : Timer
		{
			private Mobile m_Caster;
			private int m_Count;
			
			public FireSwarmTimer( Mobile caster ) : base( TimeSpan.FromSeconds( 2.0 ), TimeSpan.FromSeconds( 0.03125 ) )
			{
                                m_Caster = caster;
                                m_Caster.Frozen = true;
                                
                                Priority = TimerPriority.TenMS;

                                m_Caster.BoltEffect( 2844 );
                                Effects.SendLocationEffect( m_Caster.Location, m_Caster.Map, 0x36B0, 15, 2844, 0 );
                                Effects.PlaySound( m_Caster.Location, m_Caster.Map, 0x307 );
                        }
                        
                        protected override void OnTick()
                        {
                        	if( m_Caster == null || !m_Caster.Alive )
                                {
                        		Stop();
                        		return;
                        	}
                        	
                        	m_Count++;
                        	
                        	if( m_Count % 10 == 0 )
                        		DrawCircle( m_Caster.Location.X, m_Caster.Location.Y, m_Caster.Location.Z, 9, m_Caster.Map );

                        	int x = Utility.RandomMinMax( m_Caster.X - 5, m_Caster.X + 5 );
                                int y = Utility.RandomMinMax( m_Caster.Y - 5, m_Caster.Y + 5 );
                                int z = m_Caster.Map.GetAverageZ( x, y );
                        	Map map = m_Caster.Map;

				if( map.CanFit( x, y, z, 16, false, true ) )
                                {
                                	Effects.PlaySound( new Point3D( x, y, z ), map, 0x208 );
                                			
                                	int damlvl = 1;
                                	double rand = Utility.RandomDouble();
                                			
                                	if( 0.1 > rand )
                                	{
						Effects.SendLocationEffect( new Point3D( x, y, z ), map, 0x3709, 15, 0, 0 );
						damlvl = 3;
					}
					if( 0.3 > rand )
                                	{
						Effects.SendLocationEffect( new Point3D( x, y, z ), map, 0x36BD, 15, 0, 0 );
						damlvl = 2;
					}
					/*else
					{
                                                Effects.SendLocationEffect( new Point3D( x, y, z ), map, 0x2AE4, 15, 0, 0 );
                                        } */
						
					ArrayList arr = new ArrayList();

                                        foreach( Mobile m in map.GetMobilesInRange( new Point3D( x, y, z ), 1 ) )
					{
						if( m != null && m.Alive && m != m_Caster )
						{
							arr.Add( m );
                                        	}
                        		}
                        	
                        		foreach( Mobile m in arr )
                        		{
                        			if( damlvl == 3 )
                                			SpellHelper.Damage( TimeSpan.Zero, m, m_Caster, Utility.RandomMinMax( 20, 35 ), 0, 100, 0, 0, 0 );
                                		else if( damlvl == 2 )
                                			SpellHelper.Damage( TimeSpan.Zero, m, m_Caster, Utility.RandomMinMax( 10, 15 ), 0, 100, 0, 0, 0 );
						else
                                        		SpellHelper.Damage( TimeSpan.Zero, m, m_Caster, Utility.RandomMinMax( 4, 8 ), 0, 100, 0, 0, 0 );
                                	}
                                }
                                
                                if( m_Count >= 160 )
                                {
                                	Stop();
                                	m_Caster.Frozen = false;
                        	}
                        }
                        
                        private void DrawCircle( int xcenter, int ycenter, int z, int rad, Map map )
                        {
                        	int x = 0;
                        	int y = rad;
                        	int d = 1 - rad;

   				DrawPoints( xcenter, ycenter, x, y, z, map );

   				while ( y > x )
				{
      					if ( d < 0 )
					{
         					d += x * 2 + 3;
	 					x++;
      					}
      					else
					{
         					d += (x - y) * 2 + 5;
	 					x++;
	 					y--;
      					}

      					DrawPoints( xcenter, ycenter, x, y, z, map );
   				}
                        }
                        
                        private void DrawPoints( int xcenter, int ycenter, int x, int y, int z, Map map )
                        {
                        	Effects.SendLocationEffect( new Point3D( xcenter + x, ycenter + y, z ), map, 0x2AE4, 15, 0, 0 );
   				Effects.SendLocationEffect( new Point3D( xcenter + y, ycenter + x, z ), map, 0x2AE4, 15, 0, 0 );
      				Effects.SendLocationEffect( new Point3D( xcenter + y, ycenter - x, z ), map, 0x2AE4, 15, 0, 0 );
      				Effects.SendLocationEffect( new Point3D( xcenter + x, ycenter - y, z ), map, 0x2AE4, 15, 0, 0 );
      				Effects.SendLocationEffect( new Point3D( xcenter - x, ycenter - y, z ), map, 0x2AE4, 15, 0, 0 );
      				Effects.SendLocationEffect( new Point3D( xcenter - y, ycenter - x, z ), map, 0x2AE4, 15, 0, 0 );
      				Effects.SendLocationEffect( new Point3D( xcenter - y, ycenter + x, z ), map, 0x2AE4, 15, 0, 0 );
      				Effects.SendLocationEffect( new Point3D( xcenter - x, ycenter + y, z ), map, 0x2AE4, 15, 0, 0 );
                        }
                }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
	
	public class DemonEgg : Item, ICarvable
	{
		private SpawnTimer m_Timer;

		[Constructable]
		public DemonEgg() : base( 0x9b4 )
		{
			Movable = false;
			Hue = 2844;
			Name = "Demon Egg";

			m_Timer = new SpawnTimer( this );
			m_Timer.Start();
		}

		public void Carve( Mobile from, Item item )
		{
			Effects.PlaySound( GetWorldLocation(), Map, 0x48F );
			Effects.SendLocationEffect( GetWorldLocation(), Map, 0x3728, 10, 10, 0, 0 );

			from.SendMessage( "Has destruido el huevo." );

			Delete();

			m_Timer.Stop();
		}

		public DemonEgg( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			m_Timer = new SpawnTimer( this );
			m_Timer.Start();
		}

		private class SpawnTimer : Timer
		{
			private Item m_Item;

			public SpawnTimer( Item item ) : base( TimeSpan.FromSeconds( Utility.RandomMinMax( 10, 15 ) ) )
			{
				Priority = TimerPriority.FiftyMS;

				m_Item = item;
			}

			protected override void OnTick()
			{
				if ( m_Item.Deleted )
					return;

				double chance = Utility.RandomDouble();
				
				Mobile spawn;

				if( chance < 0.3 )
					spawn = new Daemon();
				else if( chance < 0.6 )
					spawn = new Gargoyle();
				else
					spawn = new Imp();
				
				spawn.MoveToWorld( m_Item.Location, m_Item.Map );
				spawn.Hue = 2844;
				((BaseCreature)spawn).Tamable = false;

				Effects.SendLocationEffect( m_Item.Location, m_Item.Map, 0x3709, 15, 2844, 0 );
				Effects.PlaySound( m_Item.Location, m_Item.Map, 0x208 );
				m_Item.Delete();
			}
		}
	}
}
