using System;
using System.IO;
using System.Collections;
using System.Reflection;
using Server; 
using Server.Network;
using Server.Mobiles;
using Server.Items;
using Server.Accounting;
using Server.Engines.Quests;

namespace Server.Scripts.Commands 
{ 
	public class RespSystem
	{
		private static bool m_Respalding;
		private static bool m_Restoring;

		private static string mobIdxPath = Path.Combine( "Respaldo/", "Mobiles.idx" );
		private static string mobBinPath = Path.Combine( "Respaldo/", "Mobiles.bin" );
		
		private static string itemIdxPath = Path.Combine( "Respaldo/", "Items.idx" );
		private static string itemBinPath = Path.Combine( "Respaldo/", "Items.bin" );
		
		private sealed class ItemEntry
		{
			private Item m_Item;
			private long m_Position;
			private int m_Length;

			public Item Object{ get{ return m_Item; } }
			public long Position{ get{ return m_Position; } }
			public int Length{ get{ return m_Length; } }

			public ItemEntry( Item item, long pos, int length )
			{
				m_Item = item;
				m_Position = pos;
				m_Length = length;
			}
		}

		private sealed class MobileEntry
		{
			private Mobile m_Mobile;
			private long m_Position;
			private int m_Length;

			public Mobile Object{ get{ return m_Mobile; } }
			public long Position{ get{ return m_Position; } }
			public int Length{ get{ return m_Length; } }

			public MobileEntry( Mobile mobile, long pos, int length )
			{
				m_Mobile = mobile;
				m_Position = pos;
				m_Length = length;
			}
		}

		public static void Initialize()
		{
			Server.Commands.Register( "MakeBackup", AccessLevel.Administrator, new CommandEventHandler( MakeBackup_OnCommand ) );

			m_Respalding = false;
   			m_Restoring = false;
		}

		[Usage( "MakeBackup" )]
		[Description( "Crea un respaldo de los jugadores y los items en sus bancos." )]
		private static void MakeBackup_OnCommand( CommandEventArgs e )
		{
			Backup( e.Mobile );
		}
		
		public static void Configure()
		{
			EventSink.WorldLoad += new WorldLoadEventHandler( CheckRestore );
		}
		
		public static void CheckRestore()
		{
			if( World.Mobiles.Count == 0 && World.Items.Count == 0 && File.Exists( mobIdxPath ) && File.Exists( mobBinPath ) && File.Exists( itemIdxPath ) && File.Exists( itemBinPath ) )
				Restore();
		}

		private static void SerializeMobile( Mobile m, GenericWriter idx, GenericWriter bin )
		{
                	idx.Write( m.GetType().ToString() );
                	idx.Write( (int) m.Serial );

			long start = bin.Position;
			idx.Write( (long) start );

			QuestSystem q = null;

			if( m is PlayerMobile && ((PlayerMobile)m).Quest != null )
			{
				q = ((PlayerMobile)m).Quest;
				((PlayerMobile)m).Quest = null;
			}

			m.Serialize( bin );
					
                        if( m is PlayerMobile )
				((PlayerMobile)m).Quest = q;

			idx.Write( (int) (bin.Position - start) );
			
			m.FreeCache();
		}
		
		private static void AddMobile( ArrayList list, BinaryReader idxReader )
		{
			string st = idxReader.ReadString();
			Type t = ScriptCompiler.FindTypeByFullName( st );

			if( t == null )
			{
                               	throw new Exception( String.Format( "Tipo '{0}' no esta presente en los archivos del server", st ) );
                        }

                        int serial = idxReader.ReadInt32();
			object[] ctorArgs = new object[]{ (Serial)serial };
                        Mobile m;

			try
                        {
                               	ConstructorInfo ctor = t.GetConstructor( new Type[1]{ typeof( Serial ) } );
				m = (Mobile)(ctor.Invoke( ctorArgs ));
			}
			catch
			{
				throw new Exception( String.Format( "Tipo '{0}' no tiene un constructor de serializacion", t ) );
                        }

                        long pos = idxReader.ReadInt64();
			int length = idxReader.ReadInt32();

			list.Add( new MobileEntry( m, pos, length ) );
                        World.AddMobile( m );
		}

		private static void SerializeItem( Item item, GenericWriter idx, GenericWriter bin )
		{
			idx.Write( item.GetType().ToString() );
			idx.Write( (int) item.Serial );

			long start = bin.Position;
			idx.Write( (long) start );

			item.Serialize( bin );

			idx.Write( (int) (bin.Position - start) );

			item.FreeCache();
		}
		
		private static void AddItem( ArrayList list, BinaryReader idxReader )
		{
			string st = idxReader.ReadString();
			Type t = ScriptCompiler.FindTypeByFullName( st );

			if( t == null )
			{
                               	throw new Exception( String.Format( "Tipo '{0}' no esta presente en los archivos del server", st ) );
                        }

                        int serial = idxReader.ReadInt32();
			object[] ctorArgs = new object[]{ (Serial)serial };
                        Item item;

			try
                        {
                               	ConstructorInfo ctor = t.GetConstructor( new Type[1]{ typeof( Serial ) } );
				item = (Item)(ctor.Invoke( ctorArgs ));
			}
			catch
			{
				throw new Exception( String.Format( "Tipo '{0}' no tiene un constructor de serializacion", t ) );
                        }

                        long pos = idxReader.ReadInt64();
			int length = idxReader.ReadInt32();

			list.Add( new ItemEntry( item, pos, length ) );
                        World.AddItem( item );
		}

		private static void DeserializeAll( ArrayList items, ArrayList mobiles, BinaryFileReader mobreader, BinaryFileReader itemreader )
		{
			if( items == null || mobiles == null )
				return;
				
			ArrayList itemscl = (ArrayList)items.Clone();
			ArrayList mobilescl = (ArrayList)mobiles.Clone();

			foreach( MobileEntry me in mobilescl )
			{
                        	long pos = me.Position;
                        	int length = me.Length;

				mobreader.Seek( pos, SeekOrigin.Begin );
				me.Object.Deserialize( mobreader );

				if ( mobreader.Position != ( pos + length ) )
					throw new Exception( "Error en Recuperación de Backup: Mala serialización en Item" );
					
				if( me.Object is PlayerMobile )
					InitPlayer( me.Object );

				if( me.Object.Deleted )
					mobiles.Remove( me );
			}

			foreach( ItemEntry ie in itemscl )
			{
                        	long pos = ie.Position;
                        	int length = ie.Length;

				itemreader.Seek( pos, SeekOrigin.Begin );
				ie.Object.Deserialize( itemreader );

				if ( itemreader.Position != ( pos + length ) )
					throw new Exception( "Error en Recuperación de Backup: Mala serialización en Item" );

				if( ie.Object.Deleted )
					items.Remove( ie );
			}
                }
                
                public static void InitPlayer( Mobile m )
                {
                	if( m == null )
                		return;
                		
                	for( int i = 0; i < m.Skills.Length; i++ )
                	{
                		if( m.Skills[i].Cap > 120.0 )
                                	m.Skills[i].Cap = 120.0;
                                else if( m.Skills[i].Cap < 100.0 )
                                	m.Skills[i].Cap = 100.0;
                        	
				bool psvalid = false;

				for( int j = 0; j < PowerScroll.Skills.Length && !psvalid; j++ )
				{
					if( m.Skills[i].SkillName == PowerScroll.Skills[j] )
						psvalid = true;
				}
				
				if( !psvalid )
                                	m.Skills[i].Cap = 100.0;

                                if( m.Skills[i].Base > m.Skills[i].Cap )
                                	m.Skills[i].Base = m.Skills[i].Cap;
                        }

                        if( m.Kills >= 5 )
                        {
                        	m.LogoutLocation = new Point3D( 2275, 1210, 0 );
				m.LogoutMap = Map.Felucca;
			}
			else
			{
                        	switch ( ((PlayerMobile)m).Profession )
				{
					case 4: //Necro
					{
                                        	m.LogoutLocation = new Point3D( 2114, 1301, -50 );
						m.LogoutMap = Map.Malas;
						break;
					}
					case 5:	//Paladin
					{
						m.LogoutLocation = new Point3D( 989, 520, -50 );
						m.LogoutMap = Map.Malas;
						break;
					}
					default:
					{
						switch( Utility.Random( 4 ) )
						{
							case 0:
							{
                                                        	m.LogoutLocation = new Point3D( 2466, 544, 0 );
								m.LogoutMap = Map.Felucca;
								break;
							}
							case 1:
							{
                                                        	m.LogoutLocation = new Point3D( 1867, 2780, 0 );
								m.LogoutMap = Map.Felucca;
								break;
							}
							case 2:
							{
                                                        	m.LogoutLocation = new Point3D( 4442, 1172, 0 );
								m.LogoutMap = Map.Felucca;
								break;
							}
							case 3:
							{
                                                        	m.LogoutLocation = new Point3D( 3714, 2220, 20 );
								m.LogoutMap = Map.Felucca;
								break;
							}
						}

						break;
					}
				}
			}
                }

                private static void AddToList( Mobile m, ArrayList mobs, ArrayList items )
                {
			mobs.Add( m );

			foreach( Item i in m.Items )
                        	AddToList( i, mobs, items );
                }

                private static void AddToList( Item i, ArrayList mobs, ArrayList items )
                {
			items.Add( i );
			
			if( i is ShrinkItem && ((ShrinkItem)i).Link != null )
                        	AddToList( ((ShrinkItem)i).Link, mobs, items );

			foreach( Item it in i.Items )
                        	AddToList( it, mobs, items );
                }

		private static void Backup( Mobile from )
		{
			if ( m_Respalding )
				return;

			NetState.FlushAll();
			
			m_Respalding = true;
			Console.WriteLine();
			Console.WriteLine( "Generando respaldo de Mobiles e Items..." );
			from.SendMessage( "Generando respaldo de Mobiles e Items..." );

			if ( !Directory.Exists( "Respaldo/" ) )
				Directory.CreateDirectory( "Respaldo/" );
				
			GenericWriter mobidx = new BinaryFileWriter( mobIdxPath, false );
			GenericWriter mobbin = new BinaryFileWriter( mobBinPath, true );
			GenericWriter itemidx = new BinaryFileWriter( itemIdxPath, false );
			GenericWriter itembin = new BinaryFileWriter( itemBinPath, true );
			
			ArrayList mobs = new ArrayList();
			ArrayList items = new ArrayList();

			foreach( Mobile m in World.Mobiles.Values )
			{
                		if( m is PlayerMobile )
					AddToList( m, mobs, items );
			}
			
			foreach( Item i in World.Items.Values )
			{
                		if( i is MountItem && mobs.Contains( ((MountItem)i).Mount ) )
					AddToList( i, mobs, items );
			}

                	mobidx.Write( mobs.Count );
                        itemidx.Write( items.Count );

                        foreach( Mobile m in mobs )
                        	SerializeMobile( m, mobidx, mobbin );

                        foreach( Item i in items )
                        	SerializeItem( i, itemidx, itembin );

                        mobidx.Close();
			mobbin.Close();
			itemidx.Close();
			itembin.Close();

			m_Respalding = false;

			Console.WriteLine( String.Format( "Respaldo de Mobiles e Items terminado exitosamente.\nRespaldados {0} Mobiles - {1} Items.", mobs.Count, items.Count ) );
			Console.WriteLine();
			from.SendMessage( String.Format( "Respaldo de Mobiles e Items terminado exitosamente.\nRespaldados {0} Mobiles - {1} Items.", mobs.Count, items.Count ) );
		}

		private static void Restore()
		{
			if( m_Restoring )
				return;

			m_Restoring = true;
			Console.WriteLine();
			Console.WriteLine( "Recuperando respaldo de Mobiles e Items..." );

			FileStream mobidx = new FileStream( mobIdxPath, FileMode.Open, FileAccess.Read, FileShare.Read );
			FileStream mobbin = new FileStream( mobBinPath, FileMode.Open, FileAccess.Read, FileShare.Read );
			BinaryReader mobidxReader = new BinaryReader( mobidx );
			BinaryFileReader mobreader = new BinaryFileReader( new BinaryReader( mobbin ) );

			FileStream itemidx = new FileStream( itemIdxPath, FileMode.Open, FileAccess.Read, FileShare.Read );
			FileStream itembin = new FileStream( itemBinPath, FileMode.Open, FileAccess.Read, FileShare.Read );
			BinaryReader itemidxReader = new BinaryReader( itemidx );
			BinaryFileReader itemreader = new BinaryFileReader( new BinaryReader( itembin ) );
			
			ArrayList items = new ArrayList();
			ArrayList mobiles = new ArrayList();

			int MobCount = mobidxReader.ReadInt32();
                        int ItemCount = itemidxReader.ReadInt32();

                        for( int i = 0; i < MobCount; i++ )
				AddMobile( mobiles, mobidxReader );

			for( int i = 0; i < ItemCount; i++ )
				AddItem( items, itemidxReader );

			DeserializeAll( items, mobiles, mobreader, itemreader );

			mobidxReader.Close();
			itemidxReader.Close();
			mobreader.Close();
			itemreader.Close();
			
			m_Restoring = false;

			Console.WriteLine( String.Format( "Respaldo de Mobiles e Items recuperado exitosamente.\nGenerados {0} Mobiles de {1} - {2} Items de {3}.", mobiles.Count, MobCount, items.Count, ItemCount ) );
			Console.WriteLine();
		}
	}
}
