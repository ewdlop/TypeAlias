// Ejemplo de uso de alias de tipos en español
import { Texto, Entero, Booleano, Lista, Opcional, OperaciónAsíncrona } from './español-alias-tipos';

// Uso básico
const nombre: Texto = "Juan Pérez";
const edad: Entero = 30;
const estaActivo: Booleano = true;

// Uso de colecciones
const pasatiempos: Lista<Texto> = ["leer", "programar", "jugar"];
const puntuaciones: Lista<Entero> = [95, 87, 92];

// Valores opcionales
const segundoNombre: Opcional<Texto> = null;
const teléfono: Opcional<Texto> = "555-1234";

// Función con alias de tipos
function saludar(nombrePersona: Texto, edadPersona: Entero): Texto {
    return `¡Hola, ${nombrePersona}! Tienes ${edadPersona} años.`;
}

// Ejemplo de función asíncrona
async function obtenerDatosUsuario(idUsuario: Entero): OperaciónAsíncrona<Texto> {
    // Operación asíncrona simulada
    return `Datos de usuario para ID: ${idUsuario}`;
}

// Usando las funciones
console.log(saludar(nombre, edad));

obtenerDatosUsuario(123).then((datos: Texto) => {
    console.log(datos);
});

// Exportar para uso en otros archivos
export { nombre, edad, estaActivo, pasatiempos };
