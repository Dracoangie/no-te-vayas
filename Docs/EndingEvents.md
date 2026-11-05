# Final: arrastre y fundido

En NightHouse, Player tiene un destino asignado en Events:
`Final - Destino arrastre salon`, junto a la entrada al salon.

Para iniciar el final desde un evento del Inspector, arrastra el Player de la
escena como destino y selecciona `Player > ArrastrarAlSalon()`.
El evento existente `Event - Final` conserva sus llamadas actuales; puedes
usar su On Triggered para iniciar el arrastre cuando lo decidas.

El Player bloquea los controles y se desplaza hasta el destino manteniendo su
altura y la orientacion de la camara. Drag Duration ajusta la duracion (2,8 s).
Al terminar, On Drag Finished llama a EndMenu.MostrarFin en GameMenus.
El arrastre solo puede iniciarse una vez por carga de escena.

MostrarFin tambien se puede llamar directamente desde otro evento. Cierra la
inspeccion, detiene el movimiento, bloquea la pausa y hace un fundido a negro
(1,2 s), espera un instante (0,35 s) y muestra el menu final. El fundido usa
unscaledDeltaTime para continuar aunque Time.timeScale sea cero.

GameMenus contiene End Fade, un panel negro editable y desactivado de base.
Fade Duration y Black Pause se ajustan en EndMenu. El menu se dibuja encima del
fundido una vez terminada la transicion.
