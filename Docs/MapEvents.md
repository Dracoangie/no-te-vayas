# Eventos del mapa: primer paso

`MapEventTrigger` ejecuta las acciones de `On Triggered` cuando el jugador entra en una zona. Cada zona tiene su propia configuracion.

## Crear una zona

1. Crea un GameObject vacio y llamalo, por ejemplo, `Evento - Apagar luz`.
2. Anade el componente `MapEventTrigger`. Se anade un Box Collider con `Is Trigger` activado; comprueba que siga marcado.
3. Ajusta el centro y el tamano del Box Collider para cubrir la zona de paso.
4. En `On Triggered`, pulsa `+` y arrastra la luz que quieres apagar.
5. Selecciona `GameObject > SetActive(bool)` y deja el valor desmarcado.
6. Entra en la zona con el jugador durante Play: la luz debe apagarse.

El jugador necesita un collider y un Rigidbody. El componente `Player` actual ya requiere un Rigidbody. Otros objetos no activan la zona.

## Reutilizar

- Duplica la zona, cambia su posicion y configura sus acciones para colocar mas eventos en el mapa.
- `Trigger Once` activado: la zona solo se ejecuta una vez durante esa instancia de la escena. Recargar la escena reinicia ese estado.
- `Trigger Once` desactivado: se ejecuta en cada entrada del collider del jugador. Sal y vuelve a entrar para repetirlo. Si el jugador tiene varios colliders, cada uno puede producir una entrada.
- Puedes anadir varias acciones a `On Triggered`. Se llaman en el orden de la lista, sin esperas entre ellas; sonidos y animaciones pueden continuar a la vez.
- Ejemplos de acciones: `AudioSource.Play()`, `GameObject.SetActive(bool)` o `DialogueTrigger.PlayDialogue()` de un dialogo configurado. Para este ultimo, desactiva `Play On Enter` en el DialogueTrigger si solo debe llamarse desde esta zona.
- `TriggerEvent()` tambien permite activar el evento desde otro UnityEvent.

Este primer paso no incluye condiciones de progreso, esperas ni guardado entre escenas. No hay eventos nuevos colocados en la escena: las zonas se configuran desde el Inspector.
