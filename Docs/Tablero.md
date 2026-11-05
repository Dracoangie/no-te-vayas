# Tablero reconstruido

No mantuve un tablero inicial. Esta lista está reconstruida el **5 de noviembre de 2026** a partir de los archivos y el historial. Es una fotografía de cierre documental, no una captura de cómo estaban las tareas al empezar.

**Prioridad:** P0 para recorrido, aceptación y entrega; P1 para mantenimiento y presentación. **Estimaciones iniciales:** no registradas. La columna no se rellena con tiempos inventados. Los estados describen la implementación revisada; «Hecho» no significa que toda la build haya pasado una prueba manual.

## Hecho

| ID | Tarea y descripción | Prioridad | Estimación inicial | Dependencia | Condición de cierre / evidencia |
|---|---|---|---|---|---|
| H01 | Escenario con pasillo, salón y cocina | P0 | No registrada | Proyecto Unity | Zonas montadas en NightHouse; pendiente de evaluación de ritmo |
| H02 | Movimiento, cámara y colisiones | P0 | No registrada | H01 | Player y Rigidbody configurados; controles WASD y ratón |
| H03 | Interacción común | P0 | No registrada | H02 | PlayerInteractor detecta IInteractable en nota y cofre |
| H04 | Inspección y notas | P0 | No registrada | H03 | Visor con giro/zoom y descripciones desde ItemDefinition |
| H05 | Puzzle actual de ruedas | P0 | No registrada | H04 | Seleccionar taza-radio-lámpara y validar con Abrir |
| H06 | Apertura y recogida de llave | P0 | No registrada | H05 | Tapa desplazada, llave pulsable y estado separado de recogida |
| H07 | Sonidos y eventos configurables | P1 | No registrada | H03 | SoundEvent y MapEventTrigger disponibles; conexiones en escena |
| H08 | Arrastre y fundido final | P0 | No registrada | H02, H06 | Eventos de arrastre y menú, con guardias de ejecución única |
| H09 | Pausa y recarga de escena | P0 | No registrada | H02 | Menús y funciones implementados; reinicio probado por la autora en build |
| H10 | Balanceo y pasos al caminar | P1 | No registrada | H02, H07 | Efectos activados por movimiento y detenidos al parar o bloquear controles |
| H11 | Completar una partida y probar el reinicio en build | P0 | No registrada | H01-H09 | La autora ha completado una partida y probado el reinicio en la build definitiva |
| H12 | Preparar el medio de entrega de la build definitiva | P0 | No registrada | H11 | [Release v1.0.0](https://github.com/Dracoangie/no-te-vayas/releases/tag/v1.0.0), ZIP íntegro, arranque de copia extraída y enlace desde README |

## En curso

| ID | Tarea y descripción | Prioridad | Estimación inicial | Dependencia / bloqueo | Condición para terminar |
|---|---|---|---|---|---|
| C01 | Documentación de onboarding y técnica | P0 | No registrada | Revisión de archivos | README, mapa, guía y enlaces comprobados |
| C02 | Memoria y balance de tiempo | P0 | No registrada | No se llevó un registro de horas; faltan algunos datos de validación | Dejar indicada la falta de una cifra fiable y completar las pruebas que se confirmen |
| C03 | Créditos y condiciones de uso | P0 | No registrada | Fuentes de modelos, tipografías y textura | Identificar cada recurso, licencia y condiciones de entrega |

## Pendiente

| ID | Tarea y descripción | Prioridad | Estimación inicial | Dependencia | Condición para terminar |
|---|---|---|---|---|---|
| P01 | Ajustar entrada y feedback del puzzle al enunciado | P0 | No registrada | H05 | Secuencia de tres pulsaciones, validación automática, clic, error y borrado |
| P02 | Ajustar el momento de tensión | P0 | No registrada | H04, H07 | Al cerrar la nota por primera vez: 2 s de silencio, tres pasos y cambio del mueble fuera de vista, sin repetición |
| P03 | Revisar la puerta y el desenlace exacto | P0 | No registrada | H06, H08 | Apertura y cierre con llave, frase «No te vayas todavía» y feedback requerido |
| P05 | Medir primera partida y claridad de pistas | P0 | No registrada | H11 | Hay una estimación personal de 2-3 min jugando rápido; falta registrar una primera partida sin conocer la solución |

## Orden del trabajo

El historial permite ver que primero se añadieron movimiento e interacción, después el visor y el escenario, y más tarde los sistemas de caja, audio, final y caminata. Los commits agrupados al final del desarrollo separan funcionalidades, pero no conservan un tablero diario.

No hay una captura inicial ni estimaciones previas con las que calcular desviaciones. No llevé la cuenta de las horas, así que no añado un reparto que no pueda justificar. No asigno roles ficticios: el trabajo es individual, con ayuda de Codex indicada en los créditos.
