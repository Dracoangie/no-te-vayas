# Preproducción reconstruida

**Este documento se ha preparado después de la implementación, el 5 de noviembre de 2026. No hice una preproducción ni un tablero escritos antes de empezar.** No lo presento como un registro inicial ni como una planificación que hubiera quedado guardada entonces.

## Qué quería hacer

Una experiencia corta de terror en primera persona dentro de una casa normal. El objetivo es encontrar la llave de salida relacionando una rutina familiar con tres símbolos: **taza, radio y lámpara**. La tensión debía venir del sonido y de cambios en el entorno, sin necesitar una criatura, combate ni persecuciones.

El enunciado fija una primera partida de 2 a 5 minutos y tres zonas conectadas:

```text
Puerta / recibidor / pasillo <--> Salón <--> Cocina
                                  |
                            Caja, nota y mueble
```

## Flujo que marca el diseño

| Bloque | Objetivo | Ritmo orientativo del enunciado |
|---|---|---|
| Llegada | Entender el motivo de la visita y que la salida está bloqueada | 0:00-0:25 |
| Caja y primera nota | Encontrar el objetivo y saber qué buscar | 0:25-1:00 |
| Cocina y tensión | Leer la rutina; después, pasos y cambio del mueble | 1:00-2:00 |
| Regreso al salón | Relacionar las pistas, resolver la caja y recoger la llave | 2:00-4:00 |
| Salida y final | Activar el desenlace, fundido y reinicio | 4:00-5:00 |

Estos tiempos vienen de la prueba. No son una medición de la versión realizada.

## Sistemas que necesitaba

- Movimiento y cámara de primera persona, con colisiones.
- Una interacción común para notas y caja.
- Datos de las pistas separados de su presentación.
- Un visor para examinar objetos y leer notas.
- Estado del puzzle, de la llave y de los acontecimientos ya activados.
- Sonidos, luces y cambios de objetos configurables.
- Inicio, pausa, final y reinicio.

En la implementación he usado IInteractable, ItemDefinition, prefabs y UnityEvents. La caja acabó siendo un panel de tres ruedas, la llave se recoge con un clic y el final incluye un arrastre del jugador. Son decisiones de la versión realizada; no puedo afirmar que ya estuvieran previstas en un documento inicial.

## Alcance y riesgos

El mínimo del enunciado era una partida completa con pistas legibles, solución fija, llave explícita, tensión única, salida y reinicio. Combate, guardado, IA avanzada, inventario complejo y otros niveles quedaban fuera de ese mínimo.

Los riesgos que identifico al revisar el resultado son que una conexión de eventos falle, que el jugador no entienda el orden, que se repita la tensión y que la iluminación esconda pistas. Ya he completado una partida y probado el reinicio fuera del editor, en la build definitiva. Quedan la prueba con otra persona y las condiciones de uso de algunos recursos externos.

## Estimación y comparación

No hay una estimación inicial por bloques guardada antes de empezar. Tampoco he estado contando las horas de desarrollo, así que no puedo indicar con seguridad un total ni su reparto por jornadas. No voy a convertir las cinco jornadas orientativas del enunciado en una planificación propia que no existió. Lo dejo explicado en [la memoria](MemoriaDeEjecucion.md).

El [tablero reconstruido](Tablero.md) separa lo implementado de lo que sigue pendiente. Las diferencias con las condiciones exactas de la prueba están en [Diseño y validación](DisenoYValidacion.md).
