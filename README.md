# No te vayas

He hecho una experiencia de terror en primera persona para Windows, en Unity y C#, a partir de **La última visita a casa**, la prueba de Bowl of Tentacles.

El jugador vuelve a la casa de su madre, lee dos notas y usa su rutina nocturna para abrir una caja. La solución es **taza, radio, lámpara**. He dejado tres ruedas de imágenes y un botón para comprobar la combinación. En [Diseño y validación](Docs/DisenoYValidacion.md) explico cómo funciona y qué partes no coinciden con el enunciado.

## Requisitos

- **Unity 6000.3.7f1**, revisión `696ec25a53d1`. Utiliza esta versión en Unity Hub.
- Soporte de Windows de esa instalación para generar el ejecutable. La build local utiliza Mono; si se cambia a IL2CPP, hay que instalar además su módulo y herramientas correspondientes.
- Teclado, ratón y equipo compatible con el renderizado HDRP. No se ha establecido un requisito mínimo de hardware mediante mediciones.
- Conexión durante la primera apertura para resolver los paquetes de Unity.

Dependencias principales, según `Packages/manifest.json`:

| Paquete | Versión | Uso |
|---|---|---|
| High Definition RP | 17.3.0 | Renderizado, materiales y configuración visual |
| Input System | 1.19.0 | Acciones de movimiento, cámara e interacción |
| Unity UI / UGUI | 2.0.0 | Menús, diálogos, visor y panel de la caja |
| Timeline | 1.8.11 | Paquete incluido en el proyecto; las secuencias propias se controlan con Animator y scripts |
| Test Framework | 1.6.0 | Herramientas de pruebas disponibles; no hay una suite del juego incorporada a este paquete |

El manifiesto y `packages-lock.json` contienen el listado completo. No es necesario instalar manualmente los paquetes uno a uno.

## Inicio rápido

1. Clona [el repositorio](https://github.com/Dracoangie/no-te-vayas) o extrae la entrega conservando `Assets`, `Packages`, `ProjectSettings` y todos sus `.meta`.
2. En Unity Hub, selecciona **Add project from disk** y elige la carpeta que contiene esas tres carpetas.
3. Abre con **6000.3.7f1** y deja terminar la importación y resolución de paquetes. No copies `Library` ni `Temp` de otra instalación.
4. Abre `Assets/Scenes/Intro.unity` y pulsa **Play**.
5. Inicia la partida desde el botón de la introducción. También se admiten Intro o Espacio cuando el menú está preparado.

La escena de entrada es **Intro**. Carga **NightHouse** mediante `IntroController`. `OutdoorsScene` se conserva como escena auxiliar y está desactivada en la lista actual de build.

Para editar la casa directamente, abre `Assets/Scenes/NightHouse.unity`. Para verificar la experiencia completa, vuelve a empezar desde Intro.

## Controles

| Acción | Control |
|---|---|
| Caminar | W, A, S, D |
| Mirar | Movimiento del ratón |
| Interactuar con el objeto al que se apunta | E |
| Girar un objeto en la inspección | Mantener clic izquierdo y arrastrar |
| Acercar o alejar la inspección | Rueda del ratón |
| Abrir el panel del candado | Clic sobre el candado del modelo inspeccionado |
| Cambiar una imagen | Botones de la rueda correspondiente |
| Comprobar la combinación | Botón Abrir |
| Recoger la llave | Clic sobre la llave dentro del cofre inspeccionado |
| Cerrar el panel / la inspección | Esc; primero cierra el panel del candado si está abierto |
| Pausar o reanudar durante el juego | Esc |

Las acciones se configuran en `Assets/Input/Player.inputactions`, mapa `InGame`. El jugador no puede moverse ni interactuar mientras está inspeccionando, en pausa o durante el final.

## Generar y ejecutar la build

1. Abre **File > Build Profiles** y selecciona Windows, arquitectura de 64 bits. Conserva Mono si quieres seguir la configuración de la build local. Si Unity pide un módulo de plataforma, añádelo a esa instalación desde Unity Hub.
2. Comprueba la lista de escenas: `Intro.unity` primero y `NightHouse.unity` después. `OutdoorsScene.unity` debe permanecer desactivada para esta entrega.
3. Usa **Build** y elige una carpeta de salida. La carpeta local utilizada es `Builds/` y el ejecutable se llama `No te vayas.exe`.
4. Ejecuta el `.exe` fuera del editor y completa las comprobaciones de [Diseño y validación](Docs/DisenoYValidacion.md).
5. Para distribuirlo, conserva el ejecutable junto a `No te vayas_Data`, `UnityPlayer.dll` y las dependencias generadas. No entregues únicamente el `.exe`. Excluye la carpeta de información de depuración `No te vayas_BurstDebugInformation_DoNotShip`.

**Build definitiva para Windows:** [descargar el juego en ZIP](https://github.com/Dracoangie/no-te-vayas/releases/download/v1.0.0/No-te-vayas-Windows-v1.0.0.zip) · [ver la release v1.0.0](https://github.com/Dracoangie/no-te-vayas/releases/tag/v1.0.0).

Extrae el ZIP completo y abre `No te vayas.exe` dentro de la carpeta `No-te-vayas-Windows`. No hace falta instalar Unity. Conserva todos los archivos juntos. Los enlaces **Source code** de GitHub descargan el proyecto; para jugar, utiliza `No-te-vayas-Windows-v1.0.0.zip`.

He terminado una partida completa en esta build y he probado el reinicio. También se ha comprobado la integridad del ZIP y el arranque de una copia extraída en otra carpeta. Esta comprobación de arranque no sustituye una partida completa en otro equipo. El archivo `.sha256` de la release permite verificar la descarga.

La build publicada es la versión local definitiva confirmada, sin volver a generarla durante la documentación. El código tiene cambios locales posteriores a `38330e5`; no se ha verificado que generar el proyecto de la etiqueta reproduzca exactamente este ejecutable. `Builds` está ignorada por Git, así que clonar el repositorio no descarga el juego.

## Estado y límites de la entrega

- El visor, las notas, la combinación, la recogida explícita de la llave, el movimiento final, el fundido, la pausa y el reinicio tienen implementación identificable en los scripts y prefabs.
- La caja utiliza ruedas y validación mediante Abrir, en lugar de la entrada secuencial de símbolos especificada en el enunciado. No implementa el mensaje de error, el borrado del intento ni el feedback completo exigido.
- La tercera rueda se habilita mediante un evento de progreso. La nota de cocina ejecuta sus eventos **al interactuar**, antes de abrir el visor, no al cerrarlo.
- No se ha encontrado una secuencia que garantice dos segundos de silencio y exactamente tres pasos, ni una protección específica para evitar repetir todos los eventos de esa nota.
- El final actual incluye un arrastre hacia el salón y el texto «No te vayas». No debe confundirse con una implementación confirmada de la apertura y cierre de la puerta ni con la frase exacta del enunciado.
- He terminado una partida completa en la build definitiva y he probado el reinicio. Estimo unos 2-3 minutos jugando rápido, algo más si tardo en encontrar las pistas. Quedan pendientes la prueba con otra persona y parte de las procedencias y condiciones de uso de recursos externos.

Estas observaciones no se han corregido modificando el juego durante la redacción de la documentación. El detalle y el estado de cada comprobación se encuentran en los documentos enlazados.

## Documentación

1. [Mapa técnico y decisiones](Docs/MapaTecnico.md).
2. [Guía para configurar y ampliar contenido](Docs/GuiaDeContenido.md).
3. [Diseño, diferencias y validación](Docs/DisenoYValidacion.md).
4. [Recursos externos y herramientas](Docs/RecursosYHerramientas.md).
5. [Preproducción y alcance](Docs/Preproduccion.md).
6. [Tablero de tareas](Docs/Tablero.md).
7. [Memoria de ejecución y balance](Docs/MemoriaDeEjecucion.md).
8. [Datos pendientes de confirmar](Docs/DatosPendientes.md).

He preparado la documentación con los archivos locales revisados el **5 de noviembre de 2026**, incluidos cambios posteriores al commit `38330e5`. No uso las fechas de los commits para calcular las horas ni para dar por hecha una planificación inicial.

Para consultar la interfaz de build, he tomado como referencia la [documentación de Build Profiles de Unity 6000.3](https://docs.unity.com/en-us/engine/6000.3/manual/building-and-publishing/build-settings/build-profiles-reference).
