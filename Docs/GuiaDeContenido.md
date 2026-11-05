# Guía para añadir y modificar contenido

He dejado las pistas, los sonidos y los eventos configurables desde Unity. Para cambiar ese contenido no hace falta tocar el movimiento del Player. En esta guía separo lo que puedes hacer desde el Inspector de lo que sí necesita código nuevo.

Los nombres en código se muestran tal como aparecen en los scripts. Unity presenta sus campos serializados con espacios: por ejemplo, `onInteracted` aparece como **On Interacted**.

## 1. Crear un objeto inspeccionable

**Solo configuración, usando los componentes existentes.**

1. Crea o coloca el objeto visual y añade un collider que cubra la zona seleccionable. La interacción del Player ignora colliders marcados como Trigger.
2. Añade `InspectableObject` en el objeto o un padre del collider.
3. Crea una definición desde **Create > Inspection > Item**. Configura nombre, descripción, prefab de inspección y rotación inicial.
4. Asigna esa definición al campo **Item** de InspectableObject.
5. Comprueba que hay un Player con `PlayerInteractor`, cámara asignada y alcance suficiente; la configuración base utiliza 3 unidades.
6. Comprueba que la escena contiene una instancia válida de `InspectionSystem` con su `InspectionController`, RawImage, cámara y pivote asignados.
7. Ejecuta la escena, apunta al objeto y pulsa E. Usa arrastre, rueda y Esc para comprobar el visor.

Para añadir un comportamiento que no sea inspeccionar, hace falta un componente nuevo que implemente `IInteractable.Interact()`. El PlayerInteractor ya detectará esa interfaz: no hay que duplicar su detección ni modificarla. Mantén el nuevo comportamiento en su componente y expón referencias o eventos si necesita configuración.

## 2. Añadir o modificar una pista

**Ejemplos reales:**

- `Assets/Scripts/InspectableObjects/ScriptableObjects/Note Cocina.asset`.
- `Assets/Scripts/InspectableObjects/ScriptableObjects/Note Coffer.asset`.
- Prefab compartido: `Assets/Prefabs/Note.prefab`.

Para editar una pista existente, cambia **Description** en su ItemDefinition. Al abrir la inspección, el controlador llama a `ItemDescriptionText.SetItem(item)` sobre la copia y actualiza el texto. No es necesario cambiar el texto de cada instancia de la nota manualmente.

Para crear otra pista, duplica una definición, escribe su descripción y conserva Note como Inspection Prefab. Duplica una instancia de Note en la escena y asigna la nueva definición a su InspectableObject. Configura **Initial Rotation** para que el papel se vea de frente; las pistas de cocina y cofre usan actualmente X=270, Y=0, Z=0.

El texto se presenta mediante un componente UGUI Text dentro de **Note Text Canvas > Description**. Su tamaño, tipografía, color y márgenes se editan en el prefab. `SetItem()` no actualiza continuamente el texto mientras el visor está abierto: si el contenido cambia durante esa vista, hay que llamarlo otra vez o cerrar y reabrir.

Una nota puede tener callbacks en **On Interacted**. Se ejecutan en cada interacción, antes de mostrarla. Este evento no equivale a «al cerrar la nota» y no dispone de una opción propia de ejecución única.

## 3. Configurar la caja

Abre `Assets/Prefabs/CofferKey.prefab` o su instancia en NightHouse. Revisa los overrides de la instancia: pueden reemplazar los valores del prefab.

| Campo | Configuración |
|---|---|
| Item | Definición Coffer, que indica el prefab y su rotación de inspección |
| Images | Catálogo compartido de imágenes: 0=Taza, 1=Radio, 2=Lampara |
| Combination | Índice correcto en cada rueda: X=0, Y=1, Z=2 |
| Wheel Images | Los tres RawImage donde se muestran las selecciones, en orden de izquierda a derecha |
| Lock Model / Lock Panel | Modelo pulsable y Canvas del panel; el panel debe guardarse desactivado |
| Third Wheel | Rueda funcional, inicialmente bloqueada por el estado de progreso |
| Blocked Third Wheel | Sustituto visual `Wheel 3 (1)` mientras no se habilita la tercera rueda |
| Lid | Transform de la tapa |
| Lid Open Offset / Lid Speed | Desplazamiento local y velocidad de apertura |
| Key Model | Modelo que aparece al resolver y desaparece al recogerlo |

Los PNG están en `Assets/Assets/Textures/CofferIcons`. No confundas **Images** (opciones disponibles) con **Wheel Images** (tres componentes de pantalla). En un RawImage, **UV Rect** debe ser X=0, Y=0, W=1, H=1 para mostrar la textura completa.

En **On Click** de cada rueda, Previous llama a `CofferLock.PreviousImage(int)` y Next a `NextImage(int)`. Los argumentos son 0, 1 y 2 para las tres ruedas. Abrir llama a `TryUnlock()`. Un botón de cierre puede llamar a `ClosePanel()`.

Para habilitar la tercera rueda desde un evento, arrastra **el cofre de la escena**, no el asset del prefab, y selecciona `CofferLock.EnableThirdWheel()`. Las selecciones se conservan al salir del visor.

Los eventos del cofre son distintos:

- **On Interacted:** al empezar a inspeccionarlo; puede repetirse.
- **On Unlocked:** una vez, al acertar la combinación.
- **On Key Collected:** una vez, al pulsar y recoger explícitamente la llave.

Conecta el desbloqueo de la salida a **On Key Collected**, no a On Unlocked. Resolver la caja y recoger la llave son estados diferentes.

La versión actual comprueba tres selecciones simultáneas al pulsar Abrir. Para cumplir la entrada secuencial, el borrado de intento y el feedback exacto del enunciado se necesita modificar `CofferLock` y el panel; no basta con cambiar Combination.

## 4. Activar sonido, luces y progresión

### Audio

1. Coloca `Assets/Prefabs/GameAudio.prefab` en la escena, si no existe ya una instancia adecuada.
2. Elige el hijo del sonido: Pasos, Interruptor, Intro, Efecto dramatico o Latidos.
3. En el UnityEvent que deba activarlo, pulsa +, arrastra ese hijo y elige **SoundEvent > Play()**. Para detenerlo, usa **Stop()**.
4. Ajusta Audio Clip, Volume, Loop y Spatial Blend en su AudioSource. Para sonido localizado, coloca la fuente en el punto de origen y utiliza Spatial Blend 3D.

La escena también contiene llamadas directas a **AudioSource.Play()**: son válidas, pero no tienen exactamente la política de solapamiento de SoundEvent. Una reproducción en 2D no demuestra que los pasos procedan espacialmente del pasillo.

El Player tiene su propia fuente **Walking Footsteps**, controlada por el movimiento. No la uses para el acontecimiento sobrenatural del pasillo: al parar al jugador se detiene su audio de caminar.

### Iluminación u objetos

Para un cambio sin código, añade una llamada **GameObject.SetActive(bool)** a un objeto o luz desde un evento. Configura el booleano y comprueba su estado inicial. En NightHouse ya se usa este patrón para luces de cocina y salón y para alternar instancias de un mueble.

### Zonas y diálogo

- `MapEventTrigger` necesita un BoxCollider con **Is Trigger**. Configura **Trigger Once** y **On Triggered**. Detecta un componente Player en el objeto que entra o en sus padres.
- `DialogueTrigger` expone **Dialogue**, **Duration**, **Show Once**, **Play On Enter** y **On Dialogue**.
- En On Dialogue selecciona la llamada **dinámica** a `DialogueUI.ShowDialogue(string, float)`, para que reciba el texto y duración del disparador.
- Si se va a llamar a `PlayDialogue()` desde otro evento, desactiva Play On Enter cuando no quieras que entrar en la zona también lo ejecute.

Un retraso, tres pasos exactos o un acontecimiento «al cerrar la nota» requieren una secuencia adicional o un componente de progreso. MapEventTrigger y On Interacted no realizan esas operaciones por sí solos.

### Final

`Player.ArrastrarAlSalon()` inicia una vez el desplazamiento al Transform **Drag Target**, conservando altura y orientación de cámara. En NightHouse se asigna **Events > Final - Destino arrastre salon**. **Drag Duration** controla su duración.

**On Drag Finished** llama a `EndMenu.MostrarFin()` en GameMenus. El componente EndMenu permite configurar **Fade Duration** y **Black Pause**. Su hijo **End Fade** es el panel negro, desactivado en el prefab. No conectes MostrarFin al mismo evento de inicio si quieres ver primero el arrastre completo: esa función detiene un arrastre en curso.

## 5. Añadir un segundo puzzle

No está implementado. Una ampliación razonable sería:

1. Crear un componente específico para las reglas y estado del nuevo puzzle e implementar IInteractable.
2. Crear su prefab y contenido de pistas con ItemDefinition y Note existentes.
3. Exponer su solución, referencias y un UnityEvent de resolución. Reutilizar SoundEvent, DialogueTrigger y MapEventTrigger para sus consecuencias.
4. Para una interfaz independiente, crear un Canvas propio y gestionar su apertura, cierre y bloqueo de controles. Así se reutiliza PlayerInteractor sin cambiar su raycast.
5. Si el nuevo puzzle necesita clics dentro del mismo visor 3D, generalizar la relación entre InspectionController y CofferLock mediante un contrato de interacción de vista previa. Esto **sí requiere código nuevo y cambios en el visor**.
6. Comprobar intentos erróneos, evento único, cierre de interfaz y reinicio. No almacenar el progreso mutable en el asset de ItemDefinition.

## Errores habituales

- No aparece la interacción: falta collider, es Trigger, está fuera del alcance o fuera de Interaction Layers.
- La pista no abre: falta Item o Inspection Prefab, o no existe un InspectionController válido.
- No aparece el aviso: el PlayerInteractor busca un objeto activo llamado exactamente `Interaction Panel` al iniciar.
- Las tres ruedas parecen iguales o no se ven: revisa imágenes asignadas, UV Rect y el orden de Wheel Images.
- La solución parece correcta pero no abre: comprueba Combination, los overrides de escena y que se haya ejecutado EnableThirdWheel.
- Se pierde el avance al volver al cofre: la vista previa debe recibir el original de escena mediante PreparePreview; no debe llevar un estado aislado nuevo.
- Un evento se repite al releer: On Interacted no tiene guardia de ejecución única.
- La introducción no avanza: revisa el evento FinishIntro, el Animator, `StartGame`, el estado `IntroAnim` y la escena NightHouse.
- No se muestra una función en un evento: debe ser pública y su firma debe ser compatible con ese UnityEvent.
