# Diseño y validación

## La idea del juego

He llamado al proyecto **No te vayas**. Parte de la historia de *La última visita a casa*: vuelvo a la casa de mi madre para recoger una caja y entregar las llaves. Dentro encuentro la pista de su rutina nocturna y la llave que necesito para salir.

La idea es crear tensión con una casa cotidiana, los sonidos y los cambios del entorno. No hay combate, enemigos con inteligencia artificial ni un sistema de muerte. El enunciado plantea una primera partida de **2 a 5 minutos**. He terminado una partida completa y calculo que dura unos **2 o 3 minutos si voy rápida**, y algo más si tardo en encontrar las pistas. Es una duración aproximada de mi prueba, no una validación del tiempo de una primera partida con otra persona.

## Recorrido

```text
Puerta de salida
       |
Recibidor / pasillo <--> Salón <--> Cocina
                         |           |
                   Caja y nota    Nota de la rutina
```

El jugador entra desde Intro y aparece en NightHouse. Las tres zonas forman el recorrido de la casa. La escena OutdoorsScene se conserva en el proyecto, pero no está incluida en la build actual.

1. **Llegada.** El diálogo de llegada explica que he venido a recoger la caja de mamá y después entregaré las llaves. Cerca de la salida hay una zona que muestra que está cerrada y que la llave está en su caja.
2. **Salón.** Encuentro la caja y la nota que orienta hacia los objetos de la rutina. Puedo inspeccionar el cofre, girarlo y pulsar el candado para ver sus ruedas.
3. **Cocina.** Leo la nota con la rutina. En la escena, la interacción con esta nota también reproduce los pasos, habilita la tercera rueda y cambia qué instancias del mueble están activas.
4. **Caja.** Vuelvo al salón y selecciono taza, radio y lámpara. Pulso Abrir. La tapa se desplaza y puedo pulsar la llave para recogerla.
5. **Final actual.** Recoger la llave activa `Event - Final` y desactiva `Event - PuertaLock`. Entrar en la zona final reproduce un efecto, muestra «No te vayas» e inicia el arrastre del Player hacia el salón. Al terminar se hace el fundido y aparece el menú final.

Este recorrido describe las conexiones de la escena revisada. He completado una partida en la build definitiva y he probado el reinicio. Las comprobaciones específicas que aún faltan están separadas en la tabla de validación.

## Pistas y solución

La pista del salón está en `Note Coffer.asset`:

> Para abrirla, recuerda mis noches: primero lo que preparaba en la cocina, después lo que escuchaba y, por último, la luz que dejaba encendida.

La pista de la cocina está en `Note Cocina.asset`:

> Cada noche preparaba una taza de té. Después escuchaba las noticias en la radio del salón. Antes de acostarme dejaba encendida la lámpara del pasillo.

La relación es **preparar té → taza**, **escuchar noticias → radio**, **dejar la luz → lámpara**. En Images, los índices son 0, 1 y 2, y Combination utiliza ese mismo orden.

La caja mantiene separados los estados de resolver el puzzle y recoger la llave. On Unlocked mueve la presentación del cofre; On Key Collected comunica la recogida al resto de la escena. No hay inventario visual.

## Lo que no coincide con el enunciado

Prefiero dejar estas diferencias claras antes que dar por hecho que todo está cumplido:

| Parte | Lo que pide la prueba | Lo que hay ahora |
|---|---|---|
| Entrada del puzzle | Pulsar tres símbolos como secuencia y validar el tercero | Tres ruedas independientes y comprobación con Abrir |
| Intento incorrecto | Sonido, «No era así su rutina», limpiar secuencia y permitir borrarla | El intento no abre la caja y conserva las selecciones; faltan ese feedback y el borrado |
| Pasos del pasillo | Al cerrar por primera vez la nota de cocina, esperar 2 s y reproducir exactamente tres pasos | AudioSource.Play en On Interacted, antes de abrir la inspección; no se ha encontrado esa temporización |
| Repetición de la tensión | Solo una vez por partida | La nota puede invocar de nuevo sus callbacks al releerla; EnableThirdWheel es idempotente, pero no protege el audio |
| Mueble desplazado | Una silla que cambia fuera de la vista | Se alternan dos instancias llamadas `sofa_small`; hace falta comprobar su presentación y visibilidad en la partida |
| Puerta de salida | Abrir con la llave, esperar, frase y cierre de la puerta | La llave habilita una zona de final. No se ha encontrado una secuencia propia de apertura/cierre de la puerta de salida |
| Frase final | «No te vayas todavía» | El texto configurado es «No te vayas» |
| Puesta en escena final | La puerta se cierra y se funde a negro | Hay un arrastre hacia el salón antes del fundido |
| Mensaje de recogida | «Llave de la entrada recogida» | No hay una llamada propia en CollectKey que muestre ese texto |
| Duración y prueba externa | Primera partida de 2-5 min y comprobación con alguien sin conocer la solución | La autora ha terminado una partida y estima 2-3 min jugando rápido, algo más buscando pistas; falta una primera partida externa |

La documentación no cambia estas mecánicas ni los textos del juego. Si se quiere ajustar la entrega al enunciado, estos son los puntos que habría que revisar primero.

## Comprobaciones manuales

Esta es la lista que usaría para revisar la entrega. Una función presente en el código no significa que la prueba manual esté pasada.

| Prueba | Qué comprobar | Estado de esta revisión |
|---|---|---|
| Abrir desde cero | Importar con la versión indicada, abrir Intro y llegar a NightHouse sin errores bloqueantes | Pendiente de prueba de onboarding |
| Recorrido completo | Llegar desde el inicio hasta el final | Completado por la autora en la build definitiva |
| Notas | Leer ambas en distinto orden, cerrarlas y volver a leerlas | Pendiente de confirmar esa comprobación específica |
| Error en la caja | Probar una combinación incorrecta y después la correcta | Existe reintento por ruedas; feedback y borrado del enunciado pendientes |
| Tercera rueda | Antes del evento no se mueve; después se puede usar | Implementado; comprobar la conexión de la nota en la escena |
| Solución | Taza, radio, lámpara abre una sola vez | Partida completa terminada en build; la guardia de apertura única también se comprobó de forma aislada |
| Llave | Resolver no basta: hay que pulsar la llave y esta debe desaparecer definitivamente en esa partida | Partida completa terminada en build; recogida y persistencia en la inspección comprobadas previamente de forma aislada |
| Salida sin llave | No debe iniciarse el final antes de recogerla | Revisada la activación de zonas; pendiente de confirmar la prueba sin llave |
| Tensión | Cerrar nota, 2 s de silencio, tres pasos, silla fuera de vista y sin repetición | No cumple la secuencia completa actual; pendiente |
| Final | Bloqueo de controles, un solo arrastre, fundido completo antes del menú | La autora ha completado la partida; falta confirmar la prueba de activaciones repetidas |
| Reinicio | Reiniciar la partida y comprobar que vuelve a su estado inicial | Probado por la autora en la build definitiva |
| Caminata | Pasos y balanceo al moverse; silencio al parar, pausar o inspeccionar | Comprobaciones aisladas anteriores superadas |
| Build | Ejecutar fuera del editor, terminar y reiniciar | Build definitiva confirmada por la autora, con partida completa y prueba de reinicio |
| Ritmo | Cronometrar una primera partida sin conocer la solución | Estimación personal de 2-3 min jugando rápido y algo más buscando pistas; primera partida externa pendiente |

Durante el desarrollo se realizaron comprobaciones en un proyecto temporal de Unity sobre la caja, la recogida, el arrastre, el fundido y los efectos al caminar. Esas comprobaciones usan un entorno aislado y no constituyen una suite entregada. La partida completa y el reinicio de la build son pruebas personales comunicadas por la autora. Esto no da por comprobados todos los casos específicos de aceptación ni una primera partida con otra persona.

## Cómo revisar un fallo

Primero separaría un problema de configuración de uno de lógica. Si no aparece la interacción, revisaría collider, alcance y capa. Si no abre la caja, comprobaría Images, Combination y EnableThirdWheel. Si el final llega antes de tiempo, miraría el estado inicial de Event - Final y sus callbacks. Para un error al reiniciar, repetiría la partida desde Intro y comprobaría los estados iniciales serializados.

Las incidencias que sigan pendientes deben mantenerse en [el tablero](Tablero.md), con el paso para reproducirlas y el resultado esperado.
