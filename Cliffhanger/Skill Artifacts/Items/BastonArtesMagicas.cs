// Baston de las Artes Magicas.
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
	public class BastonArtesMagicas : BaseStaff, SkillArtifact
	{   
                private bool m_AbsorbMode = false;
                public static TimeSpan delay = TimeSpan.FromSeconds( 3 );

                private bool m_LiberandoSpell = false;

                private DateTime LastCambioModo = DateTime.Now;
                //private IElfHueTimer huetimer;

                private static int Limite = 3;
                public Spell[] Almacenados = new Spell[Limite];

                public override SkillName DefSkill{ get{ return SkillName.Wrestling; } }

                private WeaponAbility m_PrimaryAbility;
                private WeaponAbility m_SecondaryAbility;
                private WeaponAnimation m_WeaponAnimation;
                private int[] BaseStaffValues = new int[10];

                public bool AbsorbMode{ get{ return m_AbsorbMode; } }
                public bool LiberandoSpell{ get{ return m_LiberandoSpell; } set{ m_LiberandoSpell = value; } }

		public override WeaponAbility PrimaryAbility{ get{ return m_PrimaryAbility; } }
		public override WeaponAbility SecondaryAbility{ get{ return m_SecondaryAbility; } }

		public override int AosStrengthReq{ get{ return BaseStaffValues[0]; } }
		public override int AosMinDamage{ get{ return BaseStaffValues[1]; } }
		public override int AosMaxDamage{ get{ return BaseStaffValues[2]; } }
		public override int AosSpeed{ get{ return BaseStaffValues[3]; } }

		public override int OldStrengthReq{ get{ return BaseStaffValues[4]; } }
		public override int OldMinDamage{ get{ return BaseStaffValues[5]; } }
		public override int OldMaxDamage{ get{ return BaseStaffValues[6]; } }
		public override int OldSpeed{ get{ return BaseStaffValues[7]; } }

		public override int InitMinHits{ get{ return BaseStaffValues[8]; } }
		public override int InitMaxHits{ get{ return BaseStaffValues[9]; } }
		
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
                        		m_TipoBase = typeof(BlackStaff);
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

			BaseStaffValues[0] = weapon.AosStrengthReq;
			BaseStaffValues[1] = weapon.AosMinDamage;
			BaseStaffValues[2] = weapon.AosMaxDamage;
			BaseStaffValues[3] = weapon.AosSpeed;

			BaseStaffValues[4] = weapon.OldStrengthReq;
			BaseStaffValues[5] = weapon.OldMinDamage;
			BaseStaffValues[6] = weapon.OldMaxDamage;
			BaseStaffValues[7] = weapon.OldSpeed;

			BaseStaffValues[8] = weapon.InitMinHits;
			BaseStaffValues[9] = weapon.InitMaxHits;
			
			m_WeaponAnimation = weapon.DefAnimation;

			Weight = weapon.Weight;
			Layer = weapon.Layer;

			weapon.Delete();
		}
		
		[Constructable]
		public BastonArtesMagicas() : this( SAUtility.TipoBaseRandom( typeof(BastonArtesMagicas) ) )
		{
		}
		
		[Constructable]
		public BastonArtesMagicas( Type tipobaston ) : base( 0xDF0 )
		{
			TipoBase = tipobaston;
			SetProps( null );
  		}

		[Constructable]
		public BastonArtesMagicas( BaseStaff BastonBase ) : base( 0x13B )
		{
			Type tipo = null;
			
			if( BastonBase != null )
				tipo = BastonBase.GetType();

			TipoBase = tipo;
			SetProps( BastonBase as BaseWeapon ); 
  		}

                public BastonArtesMagicas( Serial serial ) : base( serial )
		{
		}

 		private void SetProps( BaseWeapon ArmaBase )
		{
			if( ArmaBase != null )
                	{
				SAUtility.CopyAosAttributes( ArmaBase, this as BaseWeapon );

				//AosAttribute
                        	if( Attributes.AttackChance > 5 )
                        		Attributes.AttackChance = 5;
                        	if( Attributes.DefendChance > 10 )
                        		Attributes.DefendChance = 10;
                        	if( Attributes.BonusMana > 10 )
                        		Attributes.BonusMana = 10;
                        	if( Attributes.CastSpeed != 0 )
                                	Attributes.CastSpeed = 0;
                                
                                //AosWeaponAttribute
                       		if( WeaponAttributes.UseBestSkill == 1 )
                                	WeaponAttributes.UseBestSkill = 0;
                                if( WeaponAttributes.MageWeapon < 0 )
                                	WeaponAttributes.MageWeapon = 0;
                	}
                	
                	Hue = 1269;
                        Name = SAUtility.NombreArtefacto( GetType() );
                        Attributes.SpellChanneling = 1;
                        Attributes.SpellDamage = 5;
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
                        	from.SendMessage("Debes tener equipado el bastón para usarlo.");
                        	return;
                        }
                        
                        if( Almacenados[0] == null )
			{
				from.SendMessage("No tienes ningun Hechizo almacenado en el bastón!");
				return;
			}

                        dovPlayerMobile p = (dovPlayerMobile)from;

			if( p.BAMLastDobleClick + delay > DateTime.Now )
			{
                        	from.SendMessage("Debes esperar 3 segundos antes de usar el bastón.");
                        	return;
                        }
                        
                        if ( !SkillRequerida( from ) )
                        {
                        	from.SendMessage("Usar el bastón requiere que tengas 100 de Magery minimo.");
                        	return;
                        }
                        
                        if ( from.Region is Jail )
                        {
                        	from.SendMessage("No puedes usar el bastón aca.");
                        	return;
                        }

                        m_LiberandoSpell = true;

			if ( from != Almacenados[0].Caster )
                        {
                        	from.LocalOverheadMessage( MessageType.Regular, 1269, true, "El Hechizo no soporto la manipulacion de otra persona!");
                        	Almacenados[0].FinishSequence();
                        	return;
                        }

			from.RevealingAction();
			LiberarSpell( from, Almacenados[0] );

		}
		
		private void LiberarSpell( Mobile m, Spell spell )
		{
                        m.Paralyze( TimeSpan.FromSeconds( 1.0 ) );

                	spell.SayMantra();
			spell.State = SpellState.Sequencing;
			m.Spell = spell;

                        Target originalTarget = spell.Caster.Target;
			spell.OnCast();
			if ( spell.Caster.Player && spell.Caster.Target != originalTarget && spell.Caster.Target != null )
				spell.Caster.Target.BeginTimeout( spell.Caster, TimeSpan.FromSeconds( 30.0 ) );
		}

		public void LiberarSlot( Mobile m )
		{
			Almacenados[0] = null;

			for ( int i=0; i<Almacenados.Length-1 && Almacenados[i+1]!=null; i++ )
			{
                        	Almacenados[i] = Almacenados[i+1];
                        	Almacenados[i+1] = null;
                        }

                        m.FixedParticles( 0x37BE, 1, 15, 9961, 1269, 0, EffectLayer.Head );
                }

/************************************/
/******* HABILIDAD SECUNDARIA *******/
/************************************/

		public void Switch( Mobile m )
		{
			if ( !SkillRequerida( m ) )
                        {
                        	m.SendMessage("Este modo requiere que tengas 100 de Magery minimo.");
                        	return;
                        }

			if ( LastCambioModo + TimeSpan.FromSeconds( SACommands.DelaySwitch ) > DateTime.Now )
			{
                            	m.SendMessage("Debes esperar 3 segundos para cambiar el modo Absorcion.");
				return;
			}
				
			if ( m_AbsorbMode )
				DesactivarAbsorb( m );
                        else
                                ActivarAbsorb( m );

  		}

		private void ActivarAbsorb ( Mobile m )
		{
                	m.LocalOverheadMessage( MessageType.Regular, 1269, true, "Tu Baston comienza a irradiar energias!");
                	m.PlaySound( 488 );
                	m.FixedParticles( 0x375A, 1, 15, 9961, 1269, 0, EffectLayer.LeftHand );

                	//huetimer = new IElfHueTimer( this as Item );
                	//huetimer.Start();

                	m_AbsorbMode = true;
                        InvalidateProperties();
                        LastCambioModo = DateTime.Now;
                }


		private void DesactivarAbsorb ( Mobile m )
		{
                	m.LocalOverheadMessage( MessageType.Regular, 1269, true, "Tu Baston se normaliza.");
			m.PlaySound( 488 );
                	m.FixedParticles( 0x375A, 1, 15, 9961, 1269, 0, EffectLayer.LeftHand );

                	//this.huetimer.Stop();
                	//Hue = 1269;

                	m_AbsorbMode = false;
                        InvalidateProperties();
                        LastCambioModo = DateTime.Now;
                }
                
                public bool SkillRequerida( Mobile m )
		{
			if ( m.Skills.Magery.Value < 100.0 )
				return false;

			return true;
		}
		
		public void AbsorberSpell ( Spell abs )
		{
			int libre = 0;

			for( int i=0; i<Almacenados.Length-1 && Almacenados[i]!=null; i++ )
    				libre = i+1;

                        if ( abs.CheckSequence() )
                        {
				Almacenados[libre] = abs;
				abs.Caster.FixedParticles( 0x37C4, 1, 15, 9961, 1269, 0, EffectLayer.Head );
				abs.Caster.PlaySound( 654 );
			}

			abs.FinishSequence();

		}

/*****************************************/
/******* OTROS METODOS ADICIONALES *******/
/*****************************************/

                public override bool OnEquip( Mobile from )
                {
			if( SAUtility.MuchosEquipados( from ) )
                        {
                        	from.SendMessage( "No puedes equiparte otro artefacto." );
                        	return false;
                        }

			if ( m_AbsorbMode )
                	{
                        	if( !SkillRequerida( from ) )
                                	DesactivarAbsorb( from );
                         	else
					ActivarAbsorb( from );

                        	LastCambioModo = DateTime.Now;
                        }

                	return base.OnEquip( from );
                }
                
                public override void OnRemoved( object parent )
                {
			if ( !(parent is Mobile) )
				return;

                        if ( m_AbsorbMode )
                	{
                        	DesactivarAbsorb( (Mobile)parent );
                                m_AbsorbMode = true;
                        }
                        
                        base.OnRemoved( parent );
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			String final = "Modo Absorcion\t" + ( m_AbsorbMode ? "Activado" : "Desactivado" );

			for ( int i=0; i<=Almacenados.Length-1 && ( Almacenados[i] != null ); i++ )
                                final += "\nSpell " + (i+1).ToString() + ": " + Almacenados[i].Mantra; 

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
			
                        TipoBase = ScriptCompiler.FindTypeByName( reader.ReadString() );

			this.Hue = 1269;
		}


	}

}
