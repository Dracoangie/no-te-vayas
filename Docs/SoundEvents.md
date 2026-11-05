# Sonidos desde eventos

Arrastra `Assets/Prefabs/GameAudio.prefab` a la escena que necesite sonido.
Incluye Pasos, Interruptor, Intro, Efecto dramatico y Latidos con los cinco clips
existentes asignados. No empieza a reproducir nada automaticamente.

En cualquier evento del Inspector (On Interacted, On Triggered, On Unlocked...):

1. Pulsa +.
2. Arrastra el hijo correspondiente de GameAudio de la escena al destino.
3. Selecciona SoundEvent > Play(). Para detenerlo, selecciona Stop().

Por ejemplo: el evento de apagar la luz puede llamar a Interruptor > Play().
Un evento de peligro puede llamar a Latidos > Play(), y otro a Latidos > Stop().
Latidos tiene Loop activado y no reinicia el bucle si se vuelve a llamar Play.
Los efectos cortos permiten superponerse. Pasos reproduce el clip completo;
este sistema no sincroniza automaticamente los pasos con el movimiento.

Cada hijo contiene un AudioSource editable: Audio Clip, Volume, Loop y Spatial
Blend. Se entregan en 2D. Para sonido localizado, coloca el objeto en su origen
y cambia Spatial Blend a 3D. Cada escena necesita una camara con AudioListener.
Para un nuevo sonido, duplica un hijo, cambia su nombre y asigna su clip.
No hace falta modificar el script ni registrar el sonido en una lista.
