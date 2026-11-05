# Recursos y herramientas

He usado recursos externos para poder centrar el trabajo en la casa, la interacción y el puzzle. Aquí separo lo que he podido identificar de lo que todavía necesita una confirmación. Que un archivo esté dentro del proyecto no demuestra por sí solo su licencia.

## Iconos de la caja

Uso **coffee**, **radio-receiver** y **lamp-desk**, de [Lucide](https://lucide.dev/). Los originales proceden de [su repositorio](https://github.com/lucide-icons/lucide/tree/main/icons).

Están en `Assets/Assets/Textures/CofferIcons`, junto a su `LICENSE.txt`. Se han convertido a PNG de 256 × 256, con fondo transparente y color crema. En los SVG se sustituyó `currentColor` por `#eee1c0` porque el importador de Unity no lo admitía.

La licencia incluida contiene el aviso ISC de Lucide y el aviso MIT de los iconos derivados de Feather. Conservo el texto completo en el proyecto.

## Sonidos

| Archivo | Fuente identificada | Uso |
|---|---|---|
| `mixkit-footsteps-on-heels-on-the-pavement-542.wav` | Mixkit, efecto 542 | Pasos |
| `mixkit-on-or-off-light-switch-tap-2585.wav` | Mixkit, efecto 2585 | Interruptor |
| `mixkit-terror-sweep-of-darkness-2630.wav` | Mixkit, efecto 2630 | Efecto dramático |
| `mixkit-cinematic-heartbeat-ambience-497.wav` | Mixkit, efecto 497 | Latidos |
| `the_mountain-horror-logo-158073.mp3` | The_Mountain, [Horror Logo en Pixabay](https://pixabay.com/music/horror-scene-horror-logo-158073/) | Música de introducción |

Los archivos están en `Assets/Assets/Sounds`.

La [licencia de efectos de Mixkit](https://mixkit.co/license/modal/sfxFree/) permite incluirlos en videojuegos, pero también limita su redistribución como recursos sueltos o con archivos fuente. Antes de entregar o publicar el repositorio con los WAV originales hay que revisar esa condición; el permiso de uso dentro del juego no debe confundirse con un permiso ilimitado para redistribuir los archivos.

La música se ofrece con la [Pixabay Content License](https://pixabay.com/service/license-summary/). La ficha de Horror Logo también indica registro en Content ID. Conviene conservar la evidencia de descarga y condiciones correspondientes. No se adjunta aquí un certificado de descarga que no esté disponible.

## Modelos, materiales y texturas

| Recurso | Lo que puedo identificar | Estado de la procedencia/licencia |
|---|---|---|
| Pack de salón | Carpeta `Assets/Scenes/ZNS3D/Vintage Living Room Game Pack`; nombre **VINTAGE LIVING ROOM 3D GAME PACK**, producto 314464 en metadatos del pack | Pendiente de confirmar enlace y condiciones de adquisición/entrega |
| Cocina y otros elementos de la casa | Carpetas `Assets/Assets/Models/Cocina` y `Assets/Assets/Textures/Cocina`, junto a materiales y elementos montados en escenas | Pendiente de confirmar fuentes y licencias |
| Llave | Modelo OBJ y texturas facilitados desde la carpeta local `Downloads/key`; versión usada en `Assets/Assets/Models/Key` | Pendiente de autor, enlace original y licencia |
| Materiales y texturas adicionales | `Assets/Assets/Materials`, `Assets/Assets/Textures/Tileables` y carpetas de Living Room | Revisar su procedencia antes de dar el listado por cerrado |
| `02_madera_natural_final.png` | Imagen conservada en la raíz del repositorio | Pendiente de procedencia y condiciones de uso |

La llave se centró, se colocó en horizontal y se ajustó al tamaño del cofre. La malla usada está guardada como `Key.asset`; también se conserva el OBJ preparado, el albedo y el mapa normal. Esta preparación no cambia la autoría ni la licencia del recurso original.

No utilizo los metadatos de importación copiados como prueba de autoría de la llave: algunos importadores conservan información del recurso que se usó como plantilla.

## Tipografías

Los archivos están en `Assets/Assets/Tipografies`. Los datos siguientes se han leído de sus metadatos internos:

| Archivo | Autor o estudio indicado | Licencia |
|---|---|---|
| `Balverd Font.ttf` | Blankids Studio / Bangkit Tri Setiadi; `www.blankidsfonts.com` | Pendiente de aportar las condiciones de la descarga |
| `Morning Call.ttf` | JSH creates / Jonathan S. Harris; `jshcreates.com` | El archivo remite al acuerdo de licencia de la descarga; no se ha localizado ese acuerdo en el proyecto |
| `PHANTOM.ttf` | PINISIART / PINISIART TEAM | Pendiente de aportar las condiciones de la descarga |

No las presento como fuentes de uso libre o comercial sin tener esa información.

## Herramientas y ayuda de IA

- **Unity 6000.3.7f1 y C#:** escenas, prefabs, materiales, interfaces, lógica y build.
- **Unity Input System y UGUI:** entradas del jugador e interfaz.
- **Git:** historial y agrupación de cambios por funcionalidad. Los commits agrupados no se usan para deducir horas de trabajo.
- **Codex, de OpenAI:** se utilizó como apoyo durante la implementación y para la revisión.

Los iconos y el modelo de la llave son recursos externos, no dibujos o modelos originales generados para este proyecto por la IA. La documentación se ha contrastado con los archivos actuales y con el enunciado. Las horas, pruebas con otras personas y licencias que no se puedan comprobar se consultan a la autora en lugar de inventarlas.
