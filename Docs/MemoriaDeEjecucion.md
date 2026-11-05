# Memoria de ejecución

## Cómo he preparado esta memoria

No hice una preproducción ni un tablero escritos al empezar. Esta memoria se apoya en el historial de Git, los cambios del proyecto y las incidencias tratadas durante el desarrollo. No tomo la fecha de un commit como si fuera un registro de las horas trabajadas: varios commits se organizaron juntos al final.

No he estado contando el tiempo de desarrollo, así que no puedo indicar con seguridad las horas por jornada ni el total. No voy a rellenarlo con una estimación que no recuerde ni a tomar las cinco jornadas del enunciado como si fueran mi tiempo real.

## Hitos que sí están registrados

| Fecha del registro Git | Hito | Referencias |
|---|---|---|
| 26/10/2026 | Plantilla inicial del proyecto de terror | `9d5a8ed` |
| 27/10/2026 | Movimiento, objetos interactuables y trabajo inicial del visor | `4b911c7`, `2eb06f5`, `7f8f60b` |
| 30/10/2026 | Visor de objetos funcionando | `8e749e3` |
| 01/11/2026 | Escena base de casa, diálogos e introducción | `e3ff633`, `1cc3ca0`, `44006a4` |
| 04/11/2026 | Recursos y decoración, colisiones, inspección, pausa y menús | Grupo de commits desde `86e9495` hasta `48468a2` |
| 05/11/2026 | Cambios agrupados de eventos, audio, pistas, cofre, final y caminata | `17c95aa`, `683f110`, `45659b3`, `41cf30e`, `bb91caa`, `131ca0d`, `c708e7f`, `38330e5` |

Estas fechas abarcan varias fechas de calendario y no demuestran cuántas jornadas efectivas se dedicaron. Hay cambios locales posteriores a `38330e5`, por lo que la documentación describe también el estado local revisado.

## Decisiones y problemas conocidos

- He usado componentes y eventos configurables para no tener que escribir una respuesta distinta desde el Player para cada objeto.
- El cofre empezó con formas simples y terminó con tres ruedas, iconos y un modelo de llave. El candado y su panel están dentro del prefab para poder editarlos en Unity.
- Había que conservar el estado real del cofre aunque se cerrara su copia de inspección. La copia referencia al original, y la recogida de la llave se guarda allí durante la partida.
- Los SVG de Lucide daban un error porque Unity no admitía `currentColor` como trazo. Se cambió a un color hexadecimal explícito.
- La combinación anterior estaba configurada con índices 0, 2, 1. Al pasar a taza-radio-lámpara se ajustó a 0, 1, 2 y se revisaron los valores de la escena.
- El final necesitaba primero arrastre y después fundido y menú. El fundido utiliza tiempo no escalado para no quedarse parado cuando el juego se congela.
- Los pasos al caminar debían cortarse al parar y al abrir una interfaz. Se ligaron al movimiento real, al contacto con el suelo y al estado de controles del Player.

Son decisiones y problemas comprobables en el proyecto y en el trabajo realizado. No añado dificultades personales o tiempos que no estén confirmados.

## Comprobaciones de desarrollo

Se hicieron comprobaciones aisladas en Unity para los estados de la caja y la recogida, para el arrastre y el orden del fundido, y para el inicio y corte de los pasos y el balanceo. No son una prueba completa del recorrido ni una suite de tests incluida en la entrega. La versión local también ha recibido cambios después de esas comprobaciones.

He terminado una partida completa en la **build definitiva** y he probado el reinicio. Si voy rápida, dura unos **2 o 3 minutos**. Si tardo más en encontrar las pistas, dura algo más. Es una duración aproximada y no la presento como el resultado de una primera partida con alguien que no conoce la solución.

La build se entrega como [ZIP descargable en GitHub Releases](https://github.com/Dracoangie/no-te-vayas/releases/tag/v1.0.0), con enlace desde el README. Se ha comprobado la integridad del archivo y el arranque de una copia extraída en otra carpeta. Queda pendiente confirmar si la probó otra persona. El detalle de aceptación está en [Diseño y validación](DisenoYValidacion.md).

## Tiempo y balance

| Dato | Estado |
|---|---|
| Estimación inicial propia | No registrada |
| Preproducción y tablero iniciales | No se prepararon registros |
| Jornadas efectivas | Sin un desglose horario registrado |
| Horas por jornada y total | No llevé la cuenta; no puedo dar una cifra fiable |
| Prueba personal completa | Terminada por la autora en la build definitiva |
| Duración aproximada | 2-3 minutos jugando rápido; algo más al buscar las pistas |
| Reinicio | Probado por la autora en la build |
| Primera partida de otra persona | Pendiente de confirmar |
| Build | La versión local actual es la definitiva; partida completa y reinicio probados |
| Medio de entrega | [Release v1.0.0](https://github.com/Dracoangie/no-te-vayas/releases/tag/v1.0.0) con ZIP de Windows y archivo SHA-256 |

## Lo que queda y qué cambiaría

Hay una base de interacción y configuración reutilizable, pero la adaptación no reproduce todas las condiciones exactas del enunciado. Antes de ampliar el juego, priorizaría la entrada secuencial y feedback del puzzle, la secuencia única de tensión, el desenlace de la puerta y una prueba con alguien que no conozca la solución.

Para otro proyecto, dejaría la preproducción, las estimaciones y un registro breve al terminar cada jornada desde el principio. También cerraría la procedencia de los recursos al descargarlos y probaría antes el recorrido con alguien que no conozca la solución. Son mejoras propuestas al revisar el resultado, no decisiones que afirme haber tomado al inicio.
