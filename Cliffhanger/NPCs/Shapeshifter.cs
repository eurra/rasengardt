using System;
using System.Collections;
using System.Reflection;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "a shapeshifter corpse" )]
	public class Shapeshifter : BaseCreature
	{
		public override bool BardImmune{ get{ return true; } }
		public override bool ReaquireOnMovement{ get{ return true; } }
		public override bool AlwaysMurderer{ get{ return ( Combatant == null ); } }
		public override bool IsScaryToPets{ get{ return true; } }
		public override bool AutoDispel{ get{ return true; } }
		
		public override WeaponAbility GetWeaponAbility()
		{
			BaseWeapon weapon = Weapon as BaseWeapon;
			
			if( weapon == null )
				return null;

                        if( Skills[weapon.Skill].Base >= 70.0 )
			{
                        	switch ( Utility.Random( 2 ) )
				{
					default:
					case 0: return weapon.PrimaryAbility;
					case 1:
					{
						if( Skills[weapon.Skill].Base < 90.0 )
							goto case 0;

						return weapon.SecondaryAbility;
					}
				}
			}
			else
			{
				return null;
			}
		}

		[Constructable]
		public Shapeshifter() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.15, 0.4 )
		{
			SpeechHue = 138;
			SetDefaultProps();

			Fame = 8000;
			Karma = -8000;
		}

                private void SetDefaultProps()
                {
                	Name = "Shapeshifter";
			Title = null;
			Body = 777;
                        Hue = 16385;
                        VirtualArmor = 10;
                        
                        Str = 100;
                        Hits = HitsMax;

                        Skills.Focus.Base = 1000.0;
                        
                        AI = AIType.AI_Melee;
                }

		public Shapeshifter(Serial serial) : base(serial)
		{
		}

		public override bool CanBeHarmful( Mobile target, bool message, bool ignoreOurBlessedness )
		{
			if( target is BaseCreature )
				return false;
				
			return base.CanBeHarmful( target, message, ignoreOurBlessedness );
		}
		
		private bool ChangingAI;
		
		public override void OnCombatantChange()
		{
			base.OnCombatantChange();

			if( ChangingAI )
                        	return;

			if( !( Combatant is BaseCreature ) )
				ShapeToMobile( Combatant );
		}

		public override bool OnBeforeDeath()
		{
			ResetMorph( true );
			GenerateLoot( true );

			return base.OnBeforeDeath();
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich );
		}
		
		public override void OnThink()
		{
			base.OnThink();

			if( Combatant != null && Utility.RandomDouble() < 0.01 )
			{
				switch ( Utility.Random( 6 ) )
				{
					case 0: Say( "No puedes matar a tus enemigos y crees que podrás derrotarte tu mismo!" ); break;
					case 1: Say( "La furia de tus golpes no es más que el reflejo de tus deseos de autodestrucción..." ); break;
					case 2: Say( "{0}, Recuerda que romper a tu espejo es sinónimo de mala suerte!", Combatant.Name ); break;
					case 3: Say( "Es una lastima ser el reflejo de un debilucho como tú {0}!!", Combatant.Name ); break;
					case 4: Say( "Muahahahaha!!!" ); break;
					case 5: Say( "¿Qué se siente ser abatido por la furia de tu propia fuerza?" ); break;
				}
			}
			
			if( Skills.Healing.Base > 20.0 && Hits < HitsMax )
			{
				BandageContext context = BandageContext.GetContext( this );
				
				if( context != null )
					return;
				else
					BandageContext.BeginHeal( this, this );
			}
		}

		public override void OnGotMeleeAttack( Mobile attacker )
		{
                	base.OnGotMeleeAttack( attacker );

			CopyItems( Combatant );
                }

		private void ResetMorph( bool def )
		{
                	ArrayList copy = (ArrayList)Items.Clone();

			foreach( Item i in copy )
                        	i.Delete();

                        IMount mount = this.Mount;

			if ( mount != null )
				mount.Rider = null;

			if ( mount is Mobile )
				((Mobile)mount).Delete();

			if( def )
                        	SetDefaultProps();

                }
		
		private void CopyItems( Mobile m )
		{
			if( m == null )
				return;
				
			ResetMorph( false );
                        
                        ArrayList copy = (ArrayList)m.Items.Clone();

			foreach( Item i in copy )
			{
				if( i is IMount )
					continue;

				Type itype = i.GetType();
				Item newitem = null;

				try
				{
					newitem = Activator.CreateInstance( itype ) as Item;
				}
				catch
				{
				}

				if( newitem != null )
				{
                                	newitem.Hue = i.Hue;
                                	newitem.Name = i.Name;

                                	CopyProperties( newitem, i );

                                	if( i is BaseWeapon )
                                		SAUtility.CopyAosAttributes( (BaseWeapon)i, (BaseWeapon)newitem );
                                	else if( i is BaseArmor )
                                		SAUtility.CopyAosAttributes( (BaseArmor)i, (BaseArmor)newitem );
                                	else if( i is BaseJewel )
                                		SAUtility.CopyAosAttributes( (BaseJewel)i, (BaseJewel)newitem );
                                	else if( i is BaseClothing )
                                		SAUtility.CopyAosAttributes( (BaseClothing)i, (BaseClothing)newitem );

                                	AddItem( newitem );
				}
			}

			if( m.Mounted )
			{
				Type mtype = m.Mount.GetType();
				IMount mount = null;

				try
				{
					mount = Activator.CreateInstance( mtype ) as IMount;
				}
				catch
				{
				}

                                if( mount != null )
				{
                                	mount.Rider = this;

                                	if( m.Mount is Mobile )
                                		((Mobile)mount).Hue = ((Mobile)m.Mount).Hue;
				}
			}
		}

		private void CopyProperties ( Item dest, Item src )
		{ 
			PropertyInfo[] props = src.GetType().GetProperties(); 

			for ( int i = 0; i < props.Length; i++ ) 
			{ 
				try
				{
					if ( props[i].CanRead && props[i].CanWrite )
						props[i].SetValue( dest, props[i].GetValue( src, null ), null );
				}
				catch
				{
				}
			}
		}

		private void CopyAppareance( Mobile m )
		{
		        if( m == null )
				return;

			Name = m.Name;

			/*if( Name != null && m is dovPlayerMobile )
                        	Name += ((dovPlayerMobile)m).RaceToString(); */

                        Body = m.Body;
                        Hue = m.Hue;
                        
                        Str = m.Str;
                        Dex = m.Dex;
                        Int = m.Int;

                        Title = m.Title;
                        
                        SetResistance( ResistanceType.Physical, m.BasePhysicalResistance );
                        SetResistance( ResistanceType.Fire, m.BaseFireResistance );
                        SetResistance( ResistanceType.Cold, m.BaseColdResistance );
                        SetResistance( ResistanceType.Poison, m.BasePoisonResistance );
                        SetResistance( ResistanceType.Energy, m.BaseEnergyResistance );
                        
                        SetDamage( 15, 10 );
			SetDamageType( ResistanceType.Physical, 50 );
			SetDamageType( ResistanceType.Fire, 25 );
			SetDamageType( ResistanceType.Cold, 25 );
			SetDamageType( ResistanceType.Poison, 0 );
			SetDamageType( ResistanceType.Energy, 0 );

                        VirtualArmor = m.VirtualArmor;

                        for( int i = 0; i < Skills.Length; i++ )
                        	Skills[i].Base = m.Skills[i].Base;
                        	
                        ChangingAI = true;
			SetAI();
                }

                private void SetAI()
                {
                	AIType newAI;
                	double val = 70.0;
                	int opt = 0;

			if( Skills.Magery.Base > val )
			{
                        	val = Skills.Magery.Base;
				opt = 1;
			}
			
			if( Skills.Archery.Base > val )
			{
                        	val = Skills.Archery.Base;
				opt = 2;
			}
			
			if( Skills.Necromancy.Base > val )
			{
                        	val = Skills.Necromancy.Base;
				opt = 3;
			}
			
			if( Skills.Chivalry.Base > val )
			{
                        	val = Skills.Chivalry.Base;
				opt = 4;
			}

			switch( opt )
			{
				case 1: newAI = AIType.AI_Mage; break;
				case 2: newAI = AIType.AI_Archer; break;
				case 3: newAI = AIType.AI_Necro; break;
				case 4: newAI = AIType.AI_Paladin; break;
				default: newAI = AIType.AI_Melee; break;
			}
                        	
                        if( newAI != AI )
                        {
                        	AI = newAI;
                        	
                        	if( AI == AIType.AI_Paladin )
                        		TithingPoints = 100000;
                        }

                        ChangingAI = false;
                }
                
                private Mobile m_LastCombatant = null;
                
                [CommandProperty( AccessLevel.GameMaster )]
		public Mobile LastCombatant{ get{ return m_LastCombatant; } set{ m_LastCombatant = value; } }

		private void ShapeToMobile( Mobile m )
		{
			if( m == null )
			{
				if( m_LastCombatant != null && ( !m_LastCombatant.Alive || !InLOS( m_LastCombatant ) ) )
				{
					ResetMorph( true );
					m_LastCombatant = null;
                		}
                		
                		return;
                	}
                	else
                	{
                		if( m == m_LastCombatant )
                			return;

                		CopyAppareance( m );
                		CopyItems( m );

                        	Hits = HitsMax;
                        	Stam = StamMax;
                        	Mana = ManaMax;

                        	m_LastCombatant = m;
                        }
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}
