// Translated to Latin American - Spanish by Joe
// For corrections, please, contact me at german@bazero.biz

// Traducido al Español - Latino Americano by Joe
// Para correcciones, por favor, contactame en german@bazero.biz

// For use with Arya's Auction System 1.7 for RunUO 1.0 RC0
// Translation Revision 2


using System;
using System.Collections;

namespace Arya.Auction
{
	/// <summary>
	/// Provides access to localized text used by the system
	/// </summary>
	public class StringTable
	{
		private Hashtable m_Table;

		public StringTable()
		{
			m_Table = new Hashtable();
			m_Table.Add( 0, "Sistema de Entrega de la Subasta" );
			m_Table.Add( 1, "Entregando..." );
			m_Table.Add( 2, "Colocar el oro en tu banco" );
			m_Table.Add( 3, "Colocar el ítem en tu banco" );
			m_Table.Add( 4, "Ítem" );
			m_Table.Add( 5, "Oro" );
			m_Table.Add( 6, "Ver la pantalla de subastas" );
			m_Table.Add( 7, "Cerrar" );
			m_Table.Add( 8, "Casa de Subastas" );
			m_Table.Add( 9, "Subastar Un Ítem" );
			m_Table.Add( 10, "Ver Todas Las Subastas" );
			m_Table.Add( 11, "Ver Tus Subastas" );
			m_Table.Add( 12, "Ver Tus Ofertas" );
			m_Table.Add( 13, "Ver Pendientes" );
			m_Table.Add( 14, "Salir" );
			m_Table.Add( 15, "El sistema de subastas fue detenido.  Por favor, intente luego." );
			m_Table.Add( 16, "Buscar" );
			m_Table.Add( 17, "Ordenar" );
			m_Table.Add( 18, "Pagina {0}/{1}" ); // Pagina 1/3 - used when displaying more than one page
			m_Table.Add( 19, "Mostrando {0} ítems" ); // {0} is the number of ítems displayed in an auction listing
			m_Table.Add( 20, "No hay ítems para mostrar" );
			m_Table.Add( 21, "Pagina Previa" );
			m_Table.Add( 22, "Próxima Pagina" );
			m_Table.Add( 23, "Un error no esperado ocurrió.  Por favor, intenta de nuevo." );
			m_Table.Add( 24, "El ítem seleccionado expiro. Por favor, refresca el listado de subastas." );
			m_Table.Add( 25, "Sistema de Mensajes de la Subasta" );
			m_Table.Add( 26, "Subasta:" );
			m_Table.Add( 27, "Ver detalles" );
			m_Table.Add( 28, "No disponible" );
			m_Table.Add( 29, "Detalles del Mensaje:" );
			m_Table.Add( 30, "Tiempo disponible para que todas las partes tomen una decisión: {0} días y {1} horas." ); m_Table.Add( 31, "La subasta no existe más, por lo tanto este mensaje ya no es válido." );
			m_Table.Add( 32, "Búsquedas en la Casa de Subastas" );
			m_Table.Add( 33, "Ingresa los términos a buscar (en blanco = todos los ítems)" );
			m_Table.Add( 34, "Limitar búsqueda a estas opciones:" );
			m_Table.Add( 35, "Mapas" );
			m_Table.Add( 36, "Artefactos" );
			m_Table.Add( 37, "Power y Stat Scrolls" );
			m_Table.Add( 38, "Materias Primas" );
			m_Table.Add( 39, "Joyas" );
			m_Table.Add( 40, "Armas" );
			m_Table.Add( 41, "Armaduras" );
			m_Table.Add( 42, "Escudos" );
			m_Table.Add( 43, "Regentes" );
			m_Table.Add( 44, "Pociones" );
			m_Table.Add( 45, "BOD (Large)" );
			m_Table.Add( 46, "BOD (Small)" );
			m_Table.Add( 47, "Cancelar" );
			m_Table.Add( 48, "Buscar sólo dentro de los resultados" );
			m_Table.Add( 49, "Sistema de filtrado de la Casa de Subastas" );
			m_Table.Add( 50, "Nombre" );
			m_Table.Add( 51, "Ascendente" );
			m_Table.Add( 52, "Descendiente" );
			m_Table.Add( 53, "Fecha" );
			m_Table.Add( 54, "Más Viejo Primero" );
			m_Table.Add( 55, "Más Nuevo Primero" );
			m_Table.Add( 56, "Tiempo Restante" );
			m_Table.Add( 57, "Más Corto Primero" );
			m_Table.Add( 58, "Más Largo Primero" );
			m_Table.Add( 59, "Cantidad de Ofertas" );
			m_Table.Add( 60, "Menor Cantidad" );
			m_Table.Add( 61, "Mayor Cantidad" );
			m_Table.Add( 62, "Oferta Máxima a Realizar" );
			m_Table.Add( 63, "Más Baja Primero" );
			m_Table.Add( 64, "Más Alta Primero" );
			m_Table.Add( 65, "Máximo Permitido a Ofertar" );
			m_Table.Add( 66, "Cancelar Filtrado" );
			m_Table.Add( 67, "{0} ítem(s)" );
			m_Table.Add( 68, "Precio Inicial" );
			m_Table.Add( 69, "Reserva" );
			m_Table.Add( 70, "Oferta Más Alta" );
			m_Table.Add( 71, "Sin Ofertas" );
			m_Table.Add( 72, "Web Link" );
			m_Table.Add( 73, "{0} Dias {1} Horas" ); // 5 Days 2 Hours
			m_Table.Add( 74, "{0} Horas" ); // 18 Hours
			m_Table.Add( 75, "{0} Minutos" ); // 50 Minutes
			m_Table.Add( 76, "{0} Segundos" ); // 10 Seconds
			m_Table.Add( 77, "Pendiente" );
			m_Table.Add( 78, "No Disponible" );
			m_Table.Add( 79, "Ofertar por este ítem:" );
			m_Table.Add( 80, "Ver Ofertas" );
			m_Table.Add( 81, "Descripción del Vendedor" );
			m_Table.Add( 82, "Hue >>" );
			m_Table.Add( 83, "[los clientes 3D no muestran color]" );
			m_Table.Add( 84, "Esta subasta ha cerrado y no se aceptan más ofertas." );
			m_Table.Add( 85, "Oferta inválida.  Oferta no aceptada." );
			m_Table.Add( 86, "Historial de Ofertas" );
			m_Table.Add( 87, "¿Quién?" );
			m_Table.Add( 88, "Cantidad Ofrecida" );
			m_Table.Add( 89, "Regresar a la Subasta" );
			m_Table.Add( 90, "División Criaturas" );
			m_Table.Add( 91, "Guardar en el Establo" );
			m_Table.Add( 92, "Utiliza este ticket para guardar tu mascota en el establo." );
			m_Table.Add( 93, "Las mascotas en el establo deben ser reclamadas" ); // This and the following form one sentence
			m_Table.Add( 94, "dentro de la primera semana de haberlas guardado." );
			m_Table.Add( 95, "No pagarás por este servicio." );
			m_Table.Add( 96, "TERMINACIÓN DEL SISTEMA DE SUBASTAS" );
			m_Table.Add( 97, "<basefont color=#FFFFFF>Estás a apunto de terminar el sistema de subastas corriendo en este mundo. Esto traerá como consecuencia que todas las subastas en curso se finalicen. Todos los ítems serán retornados a sus dueños originales y los ofertantes más altos recibirán el dinero de regreso.<br><br>¿Estás seguro de querer hacer esto?" );
			m_Table.Add( 98, "SÍ, quiero terminar con el sistema" );
			m_Table.Add( 99, "NO, dejar que el sistema siga corriendo" );
			m_Table.Add( 100, "Paráemtros de Nueva Subasta" );
			m_Table.Add( 101, "Duración" );
			m_Table.Add( 102, "Días" );
			m_Table.Add( 103, "Descripción (Opcional)" );
			m_Table.Add( 104, "Web Link (Opcional)" );
			m_Table.Add( 106, "ACEPTO las normás de las subastas" ); // This and the following form one sentence
			m_Table.Add( 107, "y deseo comenzar la subasta." );
			m_Table.Add( 108, "Cancelar y salir" );
			m_Table.Add( 109, "La oferta inicial debe ser de al menos 1 moneda de oro." );
			m_Table.Add( 110, "La oferta de reserva debe ser igual o superior a la oferta inicial." );
			m_Table.Add( 111, "Una subasta debe durar al menos {0} días." );
			m_Table.Add( 112, "Una subasta no puede durar más de {0} días." );
			m_Table.Add( 113, "Por favor, especifica el nombre de esta subasta" );
			m_Table.Add( 114, "La oferta de reserva especifica es muy alta. Bajala o aumenta la oferta inicial." );
			m_Table.Add( 115, "El sistema fue cerrado" );
			m_Table.Add( 116, "El ítem llevado por este cheque no existe más por motivos ajenos al sistema de subastas" );
			m_Table.Add( 117, "El contenido del cheque fue enviado a tu cuenta de banco." );
			m_Table.Add( 118, "No se pudo agregar el ítem a tu banco.  Por favor, asegurate de tener suficiente espacio." );
			m_Table.Add( 119, "Tus poderes de Dios te permiten acceder a este cheque." );
			m_Table.Add( 120, "Este cheque sólo puede ser usado por su dueño" );
			m_Table.Add( 121, "No se supone que remuevas este ítem en forma manual.  NUNCA." );
			m_Table.Add( 122, "Cheque de Oro del Sistema de Subastas" );
			m_Table.Add( 123, "Tu oferta ha sido superada en la subasta {0}. Tu oferta fue de {1}." );
			m_Table.Add( 124, "Sistema de subastas detenido. Devolviendo tu oferta de  {1} oro por {0}" );
			m_Table.Add( 125, "La subasta para {0} fue cancelada por ti o por el dueño. Devolviéndote tu oferta." );
			m_Table.Add( 126, "Tu oferta de {0} por {1} no alcanzo el precio de reserva y el vendedor cancelo la operación." );
			m_Table.Add( 127, "La subasta estaba en estado pendiente porque no se alcanzo el precio de reserva o porque uno o más ítems fueron eliminados.  No se tomo decisión ni por parte del vendedor o del comprador, por tanto la subasta no fue exitosa.." );
			m_Table.Add( 128, "La subasta fue cancelada porque el ítem subastado fue removido del mundo.." );
			m_Table.Add( 129, "Has vendido {0} a través del sistema de subastas. La oferta más alta fue de {1}." );
			m_Table.Add( 130, "{0} no es una razón valida para un cheque del sistema de subastas" );
			m_Table.Add( 131, "Cheque de Criaturas del Sistema de Subastas" );
			m_Table.Add( 132, "Cheque de un ítem del Sistema de Subastas" );
			m_Table.Add( 133, "Tu subasta por {0} termino SIN ofertas." );
			m_Table.Add( 134, "Tu subasta por {0} fue cancelada" );
			m_Table.Add( 135, "El sistema de subastas fue detenido y el ítem subastado te fue devuelto. ({0})" );
			m_Table.Add( 136, "La subasta fue cancelada porque los ítems subastados fueron removidos del mundo." );
			m_Table.Add( 137, "Has comprado exitosamente {0} a través del sistema de subastas. Tu oferta fue de {1}." );
			m_Table.Add( 138, "{0} no es una razón valida para un cheque de ítem" );
			m_Table.Add( 139, "No puedes subastar criaturas que no te pertenezcan." );
			m_Table.Add( 140, "No puedes subastar criaturas muertas" );
			m_Table.Add( 141, "No puedes subastar criaturas summoneadas" );
			m_Table.Add( 142, "No puedes subastar familiars" );
			m_Table.Add( 143, "Por favor, vacía el backpack de la criatura primero" );
			m_Table.Add( 144, "La criatura representada por este cheque no existe más" );
			m_Table.Add( 145, "Lo siento, estamos cerrado en este momento. Por favor, intento luego nuevamente." );
			m_Table.Add( 146, "El ítem no existe más" );
			m_Table.Add( 147, "Slots de Control : {0}<br>" ); // For a pet
			m_Table.Add( 148, "Bondable : {0}<br>" );
			m_Table.Add( 149, "Fuerza : {0}<br>" );
			m_Table.Add( 150, "Agilidad : {0}<br>" );
			m_Table.Add( 151, "Inteligencia : {0}<br>" );
			m_Table.Add( 152, "Cantidad: {0}<br>" );
			m_Table.Add( 153, "Usos Disponibles : {0}<br>" );
			m_Table.Add( 154, "Spell : {0}<br>" );
			m_Table.Add( 155, "Cargas : {0}<br>" );
			m_Table.Add( 156, "Crafted by {0}<br>" );
			m_Table.Add( 157, "Material : {0}<br>" );
			m_Table.Add( 158, "Calidad : {0}<br>" );
			m_Table.Add( 159, "Hit Points : {0}/{1}<br>" );
			m_Table.Add( 160, "Durabilidad : {0}<br>" );
			m_Table.Add( 161, "Protección: {0}<br>" );
			m_Table.Add( 162, "Cargas de Veneno : {0} [{1}]<br>" );
			m_Table.Add( 163, "Rango : {0}<br>" );
			m_Table.Add( 164, "Daño : {0}<br>" );
			m_Table.Add( 165, "Precisión<br>" );
			m_Table.Add( 166, "{0} Precisión<br>" );
			m_Table.Add( 167, "Slayer : {0}<br>" );
			m_Table.Add( 168, "Mapa : {0}<br>" );
			m_Table.Add( 169, "Conteo de Spells : {0}" );
			m_Table.Add( 170, "Localización Inválida" );
			m_Table.Add( 171, "Inválida" );
			m_Table.Add( 172, "El ítem seleccionado fue removido y será guardado en estricta seguridad" );
			m_Table.Add( 173, "Cancelas la subasta y el ítem te es devuelto" );
			m_Table.Add( 174, "Cancelas la subasta y la criatura te es devuelta" );
			m_Table.Add( 175, "No tiene suficientes slots de control libres para ofertar por esa criatura" );
			m_Table.Add( 176, "Tu oferta no es lo suficientemente grande" );
			m_Table.Add( 177, "Tu oferta no alcanza el mínimo pedido" );
			m_Table.Add( 178, "Tu establo esta lleno.  Por favor, libera espacio antes de reclamar por esta criatura." );
			m_Table.Add( 179, "Tu oferta fue superada. Un cheque por {0} monedas de oro coins fue depositado en tu banco o en tu backpack. Mira los detalles de la subasta si deseas realizar otra oferta." );
			m_Table.Add( 180, "Tu subasta ha finalizado, pero la oferta máxima no alcanzó el precio de reserva especificado por ti.  Puedes tomar la decisión de si desear vender el ítem o no.<br><br>La oferta más alta es {0}. El precio de reserva era {1}." );
			m_Table.Add( 181, "<br><br>Algunos de los ítems subastados, fueron eliminados en el transcurso de esta subasta.  El comprador no aceptara la nueva subasta antes de que sea completada." );
			m_Table.Add( 182, "SÍ, deseo vender mi ítem incluso si el precio de reserva no fue alcanzado" );
			m_Table.Add( 183, "NO, no deseo vender el ítem y deseo que me sea devuelto" );
			m_Table.Add( 184, "Tu oferta no alcanzo el precio de reserva pedido por el vendedor.  El vendedor del ítem deberá ahora determinar si vende o no el ítem.<br><br>Tu oferta fue de {1}. El precio de reserva es de {2}." );
			m_Table.Add( 185, "Cerrar este mensaje" );
			m_Table.Add( 186, "Has participado y ganado la subasta. De todos modos, por razones externas uno o más ítems subastados no se encuentan disponibles. Por favor, revé la subasta utilizando el botón de detalles y decide si aun deseas comprar los ítems o no.<br><br>Tu oferta fue de {0}" );
			m_Table.Add( 187, "<br><br>Tu oferta no alcanzo el precio de reserva pedido por el vendedor.  El vendedor ahora deberá determinar si vende el ítem o no" );
			m_Table.Add( 188, "SÍ, deseo comprarlo de todos modos" );
			m_Table.Add( 189, "NO, no quiero comprar el ítem y deseo que el dinero me sea devuelto" );
			m_Table.Add( 190, "Alguno de los ítems subastados no existen más por razones externas.  El comprador ahora deberá decidir si completa la compra o no." );
			m_Table.Add( 191, "Por favor, selecciona el ítem a subastar..." );
			m_Table.Add( 192, "No puedes tener más de {0} subastas activas en tu cuenta" );
			m_Table.Add( 193, "Sólo puedes subastar ítems" );
			m_Table.Add( 194, "No puedes subastar eso" );
			m_Table.Add( 195, "Uno de los ítems a subastar no fue identificado" );
			m_Table.Add( 196, "Uno de los ítems dentro del contenedor no son aceptados por la casa de subastas" );
			m_Table.Add( 197, "No puedes vender ítems con otros ítems alojados dentro de contenedores" );
			m_Table.Add( 198, "Sólo puedes subastar ítems dentro de tu backpack o de tu cuenta bancaria" );
			m_Table.Add( 199, "No tienes suficiente dinero en el banco para colocar la oferta" );
			m_Table.Add( 200, "El sistema de subastas se encuentra detenido" );
			m_Table.Add( 201, "Eliminar" );
			m_Table.Add( 202, "Tienes una oferta en una subasta que fue cerrada por el staff.  El dinero ofertado te fue devuelto." );
			m_Table.Add( 203, "Tu subasta fue cerrada por personal del staff y el ítem te fue devuelto." );
			m_Table.Add( 204, "Tu oferta debe ser de al menos {0} más alta que la oferta actual" );
			m_Table.Add( 205, "No puedes subastar ítems que sean mobibles" );
			m_Table.Add( 206, "Propiedades" );
			m_Table.Add( 207, "La subasta seleccionada no se encuentra activa.  Por favor, refresca el listado de subastas." );
			m_Table.Add( 208, "¿Usar Comprar Ahora?:" );
			m_Table.Add( 209, "Si decides utilizar la opción de Comprar Ahora, por favor, coloca un precio superior al de Reserva" );
			m_Table.Add( 210, "Comprar este ítem ahora por {0} oro" );
			m_Table.Add( 105, @"<basefont color=#FF0000>Acuerdo de las Subastas<br>
<basefont color=#FFFFFF>Completando y enviando este formulario aseguras estar de acuerdo de formar parte del sistema de subastas.  El ítem que este subastando será removido de tu inventario y sólo te será devuelto si cancelas esta subasta, si la subasta no se completa y el ítem no es vendido, o si el staff fuerza una finalización temprana de la subasta.<br>
<basefont color=#FF0000>Oferta Inicial: <basefont color=#FFFFFF> Esta es la oferta mínima aceptada para este ítem.  Coloca un precio razonable, y posiblemente menor al que esperas recibir por este ítem al final.<br>
<basefont color=#FF0000>Reserva:<basefont color=#FFFFFF> Este valor no será conocido por los ofertantes, y debes considerarlo un precio seguro para tu ítem.  Si la oferta final alcanza este valor, la venta será automáticamente finalizada por el sistema.  Si, por otro lado, la oferta más alta esta entre la oferta inicial y el precio de reserva, se te dará la opción de poder vender este ítem o no.  Tienes 7 días, luego de la finalización de la subasta, para tomar esta decisión. Si no lo haces, el sistema de subastas asumirá que seleccionaste NO vender el ítem y te será devuelto, así como el dinero al ofertante más alto.  Los ofertantes no podrán nunca conocer el precio de reserva del ítem.  sólo recibirán una notificación cuando lo hayan alcanzado o no.<br>
<basefont color=#FF0000>Duración:<basefont color=#FFFFFF> Este valor especifica cuantos días durara la subasta desde el día de creación.  Al final de este periodo, el sistema procederá a finalizar la subasta, devolver el ítem y la oferta más alta, o esperar a tu decisión en caso que el precio de reserva no se haya alcanzado.<br>
<basefont color=#FF0000>Comprar Ahora:<basefont color=#FFFFFF> Estas opciones te permiten especificar un precio seguro al cual estés dispuesto a vender tu ítem antes de la finalización de la subasta.  Si el comprador está dispuesto a pagar este precio, podrá comprar el ítem inmediatamente sin realizar futuras ofertas.<br>
<basefont color=#FF0000>Nombre:<basefont color=#FFFFFF> Este debe ser un nombre corto definiendo el ítem que estas vendiendo.  El sistema sugerirá un nombre basado en el ítem que estas vendiendo, pero tal vez desees ponerle un nombre propio dependiendo de las circunstancias.<br>
<basefont color=#FF0000>Descripción:<basefont color=#FFFFFF> Puedes escribir prácticamente cualquier cosa que desees sobre este ítem.  Ten presentes que las propiedades de algunos ítems, que te son mostradas al colocar el puntero del mouse sobre el mismo, serán automáticamente presentadas a los ofertantes, así que no hay necesidad de colocar esos datos.  Lo mismo sucede con las monturas que este subastando: los stats y skills del animal son automáticamente colocados por el sistema.<br>
<basefont color=#FF0000>Web Link:<basefont color=#FFFFFF> Puedes agregar un link externo (en la web), para esta subasta, en caso que poseas una pagina para esto y desees brindar más información sobre el ítem subastado<br>Una vez que aceptes enviar esta subasta ten en cuenta que NO podrás acceder nuevamente al ítem subastado hasta que la subasta no finalice.  Por favor, estate completamente seguro de lo que esto significa antes de comenzar la subasta." );
			m_Table.Add( 211, "No tienes suficiente dinero en tu banco para comprar este ítem." );
			m_Table.Add( 212, "No tienes suficiente espacio en tu banco para realizar este deposito. Por favor, libera algo de espacio e inténtalo de nuevo." );
			m_Table.Add( 213, "Control de Subasta" );
			m_Table.Add( 214, "Propiedades" );
			m_Table.Add( 215, "Cuenta : {0}" );
			m_Table.Add( 216, "Dueño de la Cuenta : {0}" );
			m_Table.Add( 217, "Conectado" );
			m_Table.Add( 218, "Desconectado" );
			m_Table.Add( 219, "Ítem Subastado" );
			m_Table.Add( 220, "Colocar en tu banco" );
			m_Table.Add( 221, "Regresar el ítem al sistema" );
			m_Table.Add( 222, "Subasta" );
			m_Table.Add( 223, "Finalizar la subasta ahora" );
			m_Table.Add( 224, "Cerrar y retornar el ítem al dueño" );
			m_Table.Add( 225, "Cerrar y colocar el ítem en tu backpack" );
			m_Table.Add( 226, "Cerrar y eliminar el ítem" );
			m_Table.Add( 227, "Panel de Subastas del Staff" );
		}

		/// <summary>
		/// Gets the localized string for the Auction System
		/// </summary>
		public string this[int index]
		{
			get
			{
				string s = m_Table[ index ] as string;

				if ( s == null )
					return "Localización Faltante.  Informa a un GM.";
				else
					return s;
			}
		}
	}
}
