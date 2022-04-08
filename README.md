# Real-time audio-reactive music visualization
This is me having a go at real-time audio- (and human) reactive music visualization, made with untiy.

What I want: 
- Have Unity-scenes, midi-controllers and some real-time beat detection and frequency analysis.
- Use the midi-controllers to steer prepared or procedurally generated elements in the unity scenes. The elements might or might not be acting on their own based on the information gathered through frequency analysis and beat detection.
- Be able to improvise these "visuals" as my friends dj.

How I try to achieve this (technically):
- Using my fork of [Keijro's MidiJack plugin](https://github.com/keijiro/MidiJack), and a yet to be determined MQTT Unity PLugin I'm able to get user input from midi controllers or from an MQTT publisher into Unity.
- Using a very nice third-party programm called [Wavesum](http://wavesum.net/products.html), I get a beatclock signal (over midi) into Unity. This signal is detected in real-time listening to the microphone input of my computer.
- Using Unity's own FFT-Implentation I'm able to get a real-time frequency spectrum analysis.

Why do I want this:
- I like the idea of molding the elements of music, as you listen to it, in visual shapes and behaviours. To me, adding a second dimension to the music visually would mean to support it in telling its story.
- When I was looking for solutions to do this, the only really viable one I came across was [Magic Music Visuals](https://magicmusicvisuals.com/). It has a great community and tremendous devs, but due to some limitations (by design), I started looking further until I acknowledged, that a game engine like Unity is probably the perfect tool for me. The great advantage of Magic Music Visuals is that you don't need any programming knowledge, which was what scared me off game-engines earlier as well.

What does this mean for you:
- Nothing really, this repo is for us to have a nice Git and Github workflow, and I don't see a reason to keep the code private. Most likely the project is not going to compile or run for you though, as there are assets in use, that I'm either not willing or able to share. Have a look at the licence, use at your own risk :), as usual.
