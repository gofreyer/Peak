# Peak!
A strategy game implementation of the boardgame "Peak" by Andreas Kuhnekath-Häbler  

https://www.spielewerkstatt.eu/de/startseite/187-peak.html

https://www.jungundaltspielt.de/hoch-hinaus-peak-rezension/

The game <strong>Peak</strong>

The wooden board game "Peak" is a 2-person game by Andreas Kuhnekath-Häbler and published by Gerhards Spiel und Design.

In my preliminary first C# WPF implementation for the time being and then also in the vanilla javascript web application (developed pretty much directly from the C# code as the algorithm template) the game moves away from building towers to a more abstract way of adding up numbers.

A simple random algorithm can be set as the computer opponent at the lowest game level. For higher game levels, the minimax algorithm (with different search tree depths) can serve as a computer opponent. The minimax algorithm is an algorithm for determining the optimal game strategy for finite two-person zero-sum games with perfect information.
https://en.wikipedia.org/wiki/Minimax

<figure>
    <figcaption><strong>Human player against the computer at minimax depth 5</strong></figcaption>
    <img src="https://github.com/gofreyer/Peak/blob/main/PeakDesktop/PeakScreenShot.PNG" height="525" width="750">
</figure>

<p>Unlabelled coloured fields have a value of 1. In alternation of the colour on a 6x6 grid, exactly one origin field with the initial numerical value of 1 may be moved to exactly one other target field with a numerical value of any colour. The numerical value of the target field is then increased by 1 and it is given the colour of the original field, which itself then remains empty and colourless. As a compulsory condition, this move may only be made diagonally or orthogonally across intermediate squares (of any colour or even empty) with a total summed up value of 2. The target field has to be coloured with a value greater or equal to 1. 

By moving the numerical values in this way, gradually higher numerical values of the respective move colours are created. No move can be made from a square with a numerical value higher than 1. Thus, it is often the case that a move is made from a square only once in the game.

Those who cannot move according to the rules must pass. If the subsequent move of the other player offers new possibilities, the game continues. If not, the game ends. For each number field of a colour with a value greater than or equal to 2, one receives the respective colour points. The player who has the most wins the game.

The Syne font from https://gitlab.com/bonjour-monde/fonderie/syne-typeface is gratefully used for the web application.
