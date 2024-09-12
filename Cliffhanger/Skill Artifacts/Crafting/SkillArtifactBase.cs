// Artefacto Base.
// Hecho por Cliffhanger (archon.cl@gmail.com)

using System;
using Server;
using Server.Engines.Craft;
using Server.Targeting;

namespace Server.Items
{
	public class SkillArtifactBase : Item
	{
		private bool m_Repaired;
		private Type m_SkillArtifact;
		private Type m_TipoBase;
		
		[CommandProperty( AccessLevel.GameMaster )]
		public bool Repaired
		{
			get{ return m_Repaired; }
			set{ m_Repaired = value; InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Type SkillArtifact
		{
			get{ return m_SkillArtifact; }

			set
			{
				if( value != m_SkillArtifact )
					m_SkillArtifact = value;
				 
				UpdateItem();
			}
                }
                
                [CommandProperty( AccessLevel.GameMaster )]
		public Type TipoBase
		{
			get{ return m_TipoBase; }

			set
			{
				if( value != m_TipoBase )
					m_TipoBase = value;

				UpdateItem();
			}
                }

		[Constructable]
		public SkillArtifactBase() : this( SAUtility.TipoArtifactRandom() )
		{
		}
		
		[Constructable]
		public SkillArtifactBase( Type artifact ) : this( artifact, SAUtility.TipoBaseRandom( artifact ) )
		{
		}

		[Constructable]
		public SkillArtifactBase( Type artifact, Type tipobase ) : base( 0x13B )
		{
                        m_SkillArtifact = artifact;
			m_TipoBase = tipobase;
			
                        UpdateItem();

			Hue = 2409;
			Name = SAUtility.NombreArtefacto( m_SkillArtifact );
			Repaired = false;
		}

		public SkillArtifactBase( Serial serial ) : base( serial )
		{
		}
		
		private void UpdateItem()
		{
			if( m_SkillArtifact == null || SAUtility.BuscarInfo( m_SkillArtifact ) == null )
                        	m_SkillArtifact = SAUtility.TipoArtifactRandom();

                        if( m_TipoBase == null || !SAUtility.EsTipoBaseValido( m_SkillArtifact, m_TipoBase ) )
                        	m_TipoBase = SAUtility.TipoBaseRandom( m_SkillArtifact );
                        	
                        Item temp = Activator.CreateInstance( m_TipoBase ) as Item;

			if( temp != null )
			{
  				ItemID = temp.ItemID;
				Weight = temp.Weight;
				Layer = temp.Layer;

				temp.Delete();
			}
		}

		public int CheckRepair( Mobile from, CraftSystem cs )
		{
			if( m_TipoBase == null )
                        	return 500426;

			if( m_Repaired )
				return 1044281;

			if( !SAUtility.EsReparable( this, cs ) )
				return 500426;
			
			cs.PlayCraftEffect( from );

			if( !from.CheckSkill( cs.MainSkill, 50.0, 120.0 ) )
			{
				Delete();
				return 500424;
			}
			else
			{
                        	Repaired = true;
                        	//Hue = SAUtility.HueArtefacto( m_SkillArtifact );
                        	return 1044279;
                        }
		}
		
		public static bool CercaDeFuente( Mobile from )
		{
			if( from == null )
				return false;
				
			foreach( Item i in from.GetItemsInRange( 5 ) )
			{
				if( i is FuenteEsencial )
					return true;
			}
			
			return false;
		}
		
		public override void OnDoubleClick( Mobile from )
		{
                	if( !IsChildOf( from.Backpack ) )
                        {
                        	from.SendMessage( "El artefacto debe estar en tu bolso." );
                        	return;
                        }

			if( !m_Repaired )
			{
                        	from.SendMessage( "El artefacto aun esta en mal estado como para ser fusionado." );
                        	return;
                        }
                        
                        if( !CercaDeFuente( from ) )
                        {
                        	from.SendMessage( "Necesitas estar a unos pasos de una Fuente Esencial." );
                        	return;
                        }
                        
                        if( from.Skills[SkillName.Magery].Value < 100.0 )
                        {
                        	from.SendMessage( "El artefacto sólo puede ser fusionado por un Mago de gran poder." );
                        	return;
                        }
                        
                        from.Target = new InternalTarget( this );
                        from.SendMessage( "Selecciona el objeto que deseas fusionar al artefacto..." );
		}
		
		private class InternalTarget : Target
		{
			private SkillArtifactBase m_Artifact;
			
			public InternalTarget( SkillArtifactBase artifact ) : base( 1, false, TargetFlags.None )
			{
                        	m_Artifact = artifact;
                        }
                        
                        protected override void OnTarget( Mobile from, object targ )
			{
				if( m_Artifact.SkillArtifact == null )
					return;

				if( targ is Item )
				{
                                	if( !m_Artifact.IsChildOf( from.Backpack ) )
                        		{
                        			from.SendMessage( "El artefacto debe estar en tu bolso." );
                        			return;
                        		}
                        		
                        		if( !SkillArtifactBase.CercaDeFuente( from ) )
                        		{
                        			from.SendMessage( "Necesitas estar a unos pasos de una Fuente Esencial." );
                        			return;
                        		}
                        		
                        		if( from.Skills[SkillName.Magery].Value < 100.0 )
                        		{
                        			from.SendMessage( "El artefacto sólo puede ser fusionado por un Mago de gran poder." );
                        			return;
                        		}

					if( targ.GetType() != m_Artifact.TipoBase )
                                	{
                                        	from.SendMessage( "El objeto que seleccionaste no corresponde al tipo de tu artefacto." );
                                        	return;
                                 	}
                                 	
                                 	if( 0.1 > Utility.RandomDouble() )
                                 	{
                				from.PlaySound( 0x1EF );
						from.FixedParticles( 0x373A, 1, 15, 9961, 0, 0, EffectLayer.Waist );
						from.SendMessage( "El artefacto se rompió y no pudo fusionarse!");
						m_Artifact.Delete();
						return;
					}
                                 	
                                 	object[] args = new object[]{ targ };
                                 	Item newitem = null;

					try
					{
						newitem = (Item) Activator.CreateInstance( m_Artifact.SkillArtifact, args );
					}
					catch
					{
					}

					if( newitem != null )
					{
						if( newitem is BaseWeapon )
                                                	((BaseWeapon)newitem).Hits = ((BaseWeapon)newitem).MaxHits = Utility.RandomMinMax( ((BaseWeapon)newitem).InitMinHits, ((BaseWeapon)newitem).InitMaxHits );
                                                else if( newitem is BaseArmor )
                                                	((BaseArmor)newitem).HitPoints = ((BaseArmor)newitem).MaxHitPoints = Utility.RandomMinMax( ((BaseArmor)newitem).InitMinHits, ((BaseArmor)newitem).InitMaxHits );

						Point3D p = m_Artifact.Location;
						m_Artifact.Delete();
						((Item)targ).Delete();
						from.Backpack.DropItem( newitem );
						newitem.Location = p;
						from.PlaySound( 0x1EF );
                				from.FixedParticles( 0x373A, 1, 15, 9961, 0, 0, EffectLayer.Waist );
						from.SendMessage("El artefacto se ha fusionado al objeto seleccionado y ahora posee sus propiedades.");
     					}
    				}
				else
				{
                                	from.SendMessage("Debes seleccionar un objeto adecuado!");
    				}
   			}
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );
			list.Add( 1060660, "Estado\t" + ( m_Repaired ? "Reparado" : "No Reparado" ) );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
			
			writer.Write( m_SkillArtifact.Name );
			writer.Write( m_TipoBase.Name );
                        writer.Write( m_Repaired );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
			
			m_SkillArtifact = ScriptCompiler.FindTypeByName( reader.ReadString() );
			m_TipoBase = ScriptCompiler.FindTypeByName( reader.ReadString() );
                        m_Repaired = reader.ReadBool();
		}
	}
}
