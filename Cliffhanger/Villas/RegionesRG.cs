// Definicion de Regiones personalziadas de Rasengardt.
// Hecho por Cliffhanger (archon.cl@gmail.com)

using System;
using System.Collections;
using Server;
using Server.Mobiles;
using Server.Spells;

namespace Server.Regions
{
	public class RegionesRasengardt
	{     

		public static void Initialize()
		{
			Region.AddRegion( new RegionPK( "Cove" ) );
		}

	}
	
	
	public class RegionPK : PKRegion
	{
		public RegionPK( string name ) : this( name, typeof( GuardiaPK ) )
		{
		}

		public RegionPK( string name, Type guardType ) : base( "La ", name, Map.Felucca, guardType )
		{
		}  
	}
}
