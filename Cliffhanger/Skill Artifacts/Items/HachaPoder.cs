// Hacha del Poder.
// Hecho por Cliffhanger (archon.cl@gmail.com)

using System;
using Server.Network;
using Server.Items;
using Server.Mobiles;
using Server.Spells;
using Server.Scripts.Commands;

namespace Server.Items
{
	public class HachaPoder : BaseAxe, SkillArtifact
	{                  
                private bool m_BersMode = false;

                public static TimeSpan delay = TimeSpan.FromMinutes( 5 );
                private static int AbsTime = 10; // Tiempo del efecto de absorcion de daño en segundos.
                private DateTime LastCambioModo = DateTime.Now;
                //private IBarHueTimer huetimer;

                private WeaponAbility m_PrimaryAbility;
                private WeaponAbility m_SecondaryAbility;
                private WeaponAnimation m_WeaponAnimation;
                private int[] BaseAxeValues = new int[10];

                public bool BersMode{ get{ return m_BersMode; } }

		public override WeaponAbility PrimaryAbility{ get{ return m_PrimaryAbility; } }
		public override WeaponAbility SecondaryAbility{ get{ return m_SecondaryAbility; } }

		public override int AosStrengthReq{ get{ return BaseAxeValues[0]; } }
		public override int AosMinDamage{ get{ return BaseAxeValues[1]; } }
		public override int AosMaxDamage{ get{ return BaseAxeValues[2]; } }
		public override int AosSpeed{ get{ return BaseAxeValues[3]; } }

		public override int OldStrengthReq{ get{ return BaseAxeValues[4]; } }
		public override int OldMinDamage{ get{ return BaseAxeValues[5]; } }
		public override int OldMaxDamage{ get{ return BaseAxeValues[6]; } }
		public override int OldSpeed{ get{ return BaseAxeValues[7]; } }

		public override int InitMinHits{ get{ return BaseAxeValues[8]; } }
		public override int InitMaxHits{ get{ return BaseAxeValues[9]; } }
		
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
                        		m_TipoBase = typeof(Axe);
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

			BaseAxeValues[0] = weapon.AosStrengthReq;
			BaseAxeValues[1] = weapon.AosMinDamage;
			BaseAxeValues[2] = weapon.AosMaxDamage;
			BaseAxeValues[3] = weapon.AosSpeed;

			BaseAxeValues[4] = weapon.OldStrengthReq;
			BaseAxeValues[5] = weapon.OldMinDamage;
			BaseAxeValues[6] = weapon.OldMaxDamage;
			BaseAxeValues[7] = weapon.OldSpeed;

			BaseAxeValues[8] = weapon.InitMinHits;
			BaseAxeValues[9] = weapon.InitMaxHits;
			
			m_WeaponAnimation = weapon.DefAnimation;

			Weight = weapon.Weight;
			Layer = weapon.Layer;

			weapon.Delete();
		}

		[Constructable]
		public HachaPoder() : this( SAUtility.TipoBaseRandom( typeof(HachaPoder) ) )
		{
		}
		
		[Constructable]
		public HachaPoder( Type tipohacha ) : base( 0x13B )
		{
			TipoBase = tipohacha;
			SetProps( null );
  		}
  		
		public HachaPoder( BaseAxe HachaBase ) : base( 0x13B )
		{
			Type tipo = null;
			
			if( HachaBase != null )
				tipo = HachaBase.GetType();

			TipoBase = tipo;
			SetProps( HachaBase as BaseWeapon );
		}

                public HachaPoder( Serial serial ) : base( serial )
		{
		}

 		private void SetProps( BaseWeapon ArmaBase )
		{
			if( ArmaBase != null )
                	{
				SAUtility.CopyAosAttributes( ArmaBase, this as BaseWeapon );

                        	//AosAttribute
                        	if( Attributes.WeaponSpeed > 15 )
                        		Attributes.WeaponSpeed = 15;
                        	if( Attributes.WeaponDamage > 30 )
                        		Attributes.WeaponDamage = 30;
                        	if( Attributes.AttackChance > 10 )
                        		Attributes.AttackChance = 10;
                        	if( Attributes.DefendChance > 10 )
                        		Attributes.DefendChance = 10;

                        	//AosWeaponAttribute
                        	if( WeaponAttributes.UseBestSkill == 1 )
                                	WeaponAttributes.UseBestSkill = 0;
                                if( WeaponAttributes.MageWeapon < 0 )
                                	WeaponAttributes.MageWeapon = 0;
                	}
                	
                	Hue = 1357;
                        Name = SAUtility.NombreArtefacto( GetType() );
                        Attributes.BonusStam = 5;
                        Attributes.BonusMana = 5;
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
                        	from.SendMessage("Debes tener equipada la Hacha para usarla.");
                        	return;
                        }

                        if( m_BersMode )
                        {
                        	from.SendMessage("No puedes usar esta habilidad en modo Berserker.");
                        	return;
                        }

			if( ((dovPlayerMobile)from).HPLastDobleClick + TimeSpan.FromMinutes( 5 ) > DateTime.Now )
			{
                        	from.SendMessage("Debes esperar 5 Minutos antes de usar nuevamente el Hacha.");
                        	return;
                        }
			
			if ( !SkillRequerida( from ) )
                        {
                        	from.SendMessage("Usar el hacha requiere que tengas 100 de Swordsmanship minimo.");
                        	return;
                        }
                        
                        from.RevealingAction();
			SetAbs( from, true );
		}
		
		private bool m_AbsMode;
		private int ExtraHealth = 0;
		private InternalTimer timer;
		
		public bool AbsMode{ get{ return m_AbsMode; } }
		
		public int AbsorverDam( int damage, Mobile m )
		{
                	if( m == null )
                		return damage;

			m.FixedParticles( 0x36FE, 1, 10, 9910, 1357, 0, EffectLayer.LeftHand );

			if( ExtraHealth >= damage )
                	{
                        	ExtraHealth -= damage;
                        	return 0;
                        }
                        else
                        {
                                damage -= ExtraHealth;
				SetAbs( m, false );

				return damage;
			}
		}
		
		private void SetAbs( Mobile from, bool On )
		{
			if( from == null )
				return;

			if( On )
			{
				m_AbsMode = true;
				ExtraHealth = from.HitsMax;

				timer = new InternalTimer( from, this );
				timer.Start();

				from.LocalOverheadMessage( MessageType.Regular, 1357, true, "El poder del Hacha te protege!");
                        	from.PlaySound( 481 );
                        	from.FixedParticles( 0x375A, 1, 15, 9961, 1357, 0, EffectLayer.LeftHand );

                        	((dovPlayerMobile)from).HPLastDobleClick = DateTime.Now;
                        }
                        else
                        {
                        	m_AbsMode = false;
				ExtraHealth = 0;

				if( timer != null )
				{
					timer.Stop();
					timer = null;
				}

				from.LocalOverheadMessage( MessageType.Regular, 1357, true, "Vuelves a la Normalidad" );
				from.FixedParticles( 0x375A, 1, 15, 9961, 1357, 0, EffectLayer.LeftHand );
                        	from.PlaySound( 481 );
                        }
  		}
  		
  		private class InternalTimer : Timer
		{
			private Mobile m_From;
			private HachaPoder m_Hacha;

			public InternalTimer( Mobile m, HachaPoder hacha ) : base( TimeSpan.FromSeconds( HachaPoder.AbsTime ) )
			{
                        	m_From = m;
                                m_Hacha = hacha;
                        }

                        protected override void OnTick()
                        {
                        	if( m_From == null || m_Hacha == null || m_Hacha.Deleted )
                        		return;
                        		
                        	m_Hacha.SetAbs( m_From, false );
                        }

                }
  		
/************************************/
/******* HABILIDAD SECUNDARIA *******/
/************************************/

		public void Switch( Mobile m )
		{
			if ( !SkillRequerida( m ) )
                        {
                        	m.SendMessage("Este modo requiere que tengas 100 de Swordsmanship minimo.");
                        	return;
                        }

			if ( LastCambioModo + TimeSpan.FromSeconds( SACommands.DelaySwitch ) > DateTime.Now )
			{
                            	m.SendMessage("Debes esperar 3 segundos para cambiar tu estado de Berserker.");
				return;
			}
			
			if( m_AbsMode )
                        {
                        	m.SendMessage("No puedes transformarte en tu estado actual.");
                        	return;
                        }
				
			if ( m_BersMode )
				DesactivarBers( m, false );
                        else
                                ActivarBers( m, false );
  		}

		private void ActivarBers( Mobile m, bool OnEquip )
		{
                	m.LocalOverheadMessage( MessageType.Regular, 1357, true, "El poder de la furia invade tus venas!");
                	m.PlaySound( 903 );
                	m.FixedParticles( 0x37B9, 1, 25, 9961, 1357, 0, EffectLayer.LeftHand );

                	if( !OnEquip )
                	{
                        	m_FuryLevel = 0;
                        	m_FuryTimer = new InternalTimer2( this );
                                m_FuryTimer.Start();
                	}

                	//huetimer = new IBarHueTimer( this as Item );
                	//huetimer.Start();
                	
                        AgregarEfectos( m );
			m_BersMode = true;
                        InvalidateProperties();
                        LastCambioModo = DateTime.Now;
                }

		private void DesactivarBers( Mobile m, bool OnEquip )
		{
                	m.LocalOverheadMessage( MessageType.Regular, 1357, true, "Vuelves a la normalidad");
			m.PlaySound( 903 );
                	m.FixedParticles( 0x37B9, 1, 25, 9961, 1357, 0, EffectLayer.LeftHand );

                	//this.huetimer.Stop();
                	//Hue = 1357;
                	
                	if( !OnEquip )
                	{
                        	if( m_FuryTimer != null )
                        	{
					m_FuryTimer.Stop();
                                	m_FuryTimer = null;
                                }
                                
                                rmod1.Offset = rmod2.Offset = rmod3.Offset = rmod4.Offset = rmod5.Offset = 0;
				smod.Value = 0;
                	}
                	
                	if ( smod != null )
				smod.Remove();
                	
                        RemoverResistencias( m );
                	m_BersMode = false;
                        InvalidateProperties();
                        LastCambioModo = DateTime.Now;
                }
                
                private int m_FuryLevel;
		private InternalTimer2 m_FuryTimer;
                
                // Cantidad máxima de puntos de furia.
		private static int FuryLevelLimit = 25;

		public int FuryLevel
		{
			get{ return m_FuryLevel; }

			set
			{
                        	m_FuryLevel = value;
			
				if( m_FuryLevel > FuryLevelLimit )
                        		m_FuryLevel = FuryLevelLimit;
                        	else if( m_FuryLevel < 0 )
                        		m_FuryLevel = 0;

                        	RemoverResistencias( Parent as Mobile );
                        	AgregarEfectos( Parent as Mobile );
                        	
                        	InvalidateProperties();
                        }
                }

		private ResistanceMod rmod1 = new ResistanceMod( ResistanceType.Physical, 0 );
                private ResistanceMod rmod2 = new ResistanceMod( ResistanceType.Fire, 0 );
                private ResistanceMod rmod3 = new ResistanceMod( ResistanceType.Cold, 0 );
                private ResistanceMod rmod4 = new ResistanceMod( ResistanceType.Poison, 0 );
                private ResistanceMod rmod5 = new ResistanceMod( ResistanceType.Energy, 0 );
                
                private SkillMod smod = new DefaultSkillMod( SkillName.Focus, true, 0 );

                private void RemoverResistencias( Mobile m )
                {
                	if ( m == null )
                 		return;

			m.RemoveResistanceMod( rmod1 );
                        m.RemoveResistanceMod( rmod2 );
                        m.RemoveResistanceMod( rmod3 );
                        m.RemoveResistanceMod( rmod4 );
                        m.RemoveResistanceMod( rmod5 );
                }

                private void AgregarEfectos( Mobile m )
                {
                	if ( m == null )
                 		return;

                	rmod1.Offset = rmod2.Offset = rmod3.Offset = rmod4.Offset = rmod5.Offset = (int)( -( m_FuryLevel * 0.5 ) );
			smod.Value = -(m_FuryLevel * 1.5);
                	
                	m.AddResistanceMod( rmod1 );
                 	m.AddResistanceMod( rmod2 );
                 	m.AddResistanceMod( rmod3 );
                 	m.AddResistanceMod( rmod4 );
                 	m.AddResistanceMod( rmod5 );
                 	
                 	m.AddSkillMod( smod );
                }
                
                public int GetDamBonus{ get{ return (int) (m_FuryLevel * 2.0); } }
                public int GetHitChBonus{ get{ return m_FuryLevel; } }
                public int GetDelayBonus{ get{ return (int) (m_FuryLevel * 1.2); } }
                public int GetDefChanceCost{ get{ return -m_FuryLevel; } }
		
		public override void OnHit( Mobile attacker, Mobile defender )
		{
			base.OnHit( attacker, defender );

			if( m_BersMode )
                        	FuryLevel += Utility.RandomMinMax( 4, 8 );
		}
		
		public bool SkillRequerida( Mobile m )
		{
			if ( m.Skills.Swords.Value < 100.0 )
				return false;
			
			return true;
		}
		
		private class InternalTimer2 : Timer
		{
			private HachaPoder m_Hacha;

			public InternalTimer2( HachaPoder hacha ) : base( TimeSpan.FromSeconds( 10.0 ), TimeSpan.FromSeconds( 10.0 ) )
			{
                                m_Hacha = hacha;
                        }

                        protected override void OnTick()
                        {
                        	if( m_Hacha == null || m_Hacha.Deleted )
                        	{
                        		Stop();
                        		return;
                        	}

                                m_Hacha.FuryLevel--;
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
				pois = cold = 0;
				nrgy = 50;
				fire = 25;
				phys = 25;
			}
		}
 
                public override bool OnEquip( Mobile from )
                {
			if( SAUtility.MuchosEquipados( from ) )
                        {
                        	from.SendMessage( "No puedes equiparte otro artefacto." );
                        	return false;
                        }

			if ( m_BersMode )
                	{
                       		if( !SkillRequerida( from ) )
                                	DesactivarBers( from, true );
                         	else
					ActivarBers( from, true );

                        	LastCambioModo = DateTime.Now;
                        }

                	return base.OnEquip( from );
                }
                
                public override void OnRemoved( object parent )
                {
			if ( !(parent is Mobile) )
				return;

                        if ( m_BersMode )
                        {
                        	DesactivarBers( (Mobile)parent, true );
                                m_BersMode = true;
                        }
                        
                        if( m_AbsMode )
                        	SetAbs( (Mobile)parent, false );

                        base.OnRemoved( parent );
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );
			list.Add( 1060660, "Modo Berserker\t" + ( m_BersMode ? "Activado\nPuntos de Furia : " + m_FuryLevel.ToString() : "Desactivado" ) );
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

			this.Hue = 1357;
		}

	}

}
