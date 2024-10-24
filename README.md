# Oefening 5: Snake

Maak het spel Snake met behulp van generieke Lists, de Point klasse, DispatcherTimer en de Canvas layout.

![Afbeelding met schermopname, software, Multimediasoftware,
Besturingssysteem Automatisch gegenereerde
beschrijving](./media/image1.png)

**Uitleg** :

1.  Canvas:
Canvas laat ons toe om het venster te structureren in coördinaten.
Hiervoor gebruiken we de Canvas.SetBottom en Canvas.SetLeft methodes om de elementen in het Canvas te positioneren.

2.  Point:
De Point klasse kan gebruikt worden om de coördinaten van de elementen bij te houden die verschijnen op het scherm. Een Point bestaat zowel uit een X als een Y waarde.

Voorbeeldcode:

```cs
Point p = new Point();
int x = 10;
int y = 10;
Point pointWithValues = new Point(x, y);
p.X = x;
```

3.  DispatcherTimer:
De DispatcherTimer laat ons toe om de slang te laten bewegen over het scherm. Meer specifiek zal de dispatcher er voor zorgen dat we iedere "game-tick" de huidige status van het scherm beschrijven en tekenen.

4.  Lists:
Met lists kunnen we bijhouden welke elementen er op het scherm staan. Verder laat het ons toe om makkelijk nieuwe elementen toe te voegen en te verwijderen.

**Stappenplan** :

1.  Tekenen van een Slang:
    a.  Lokale Variabelen:
-   List van Points (List\<Point\>) die het lichaam van de slang voorstellen. De punten representeren de X en Y coördinaten die gebruikt worden met de Canvas methodes SetBottom (= marge ten opzichte van de onderkant) en SetLeft (= marge ten opzichte van de linkerkant)
-   De DispatcherTimer zal er voor zorgen dat we iedere X milliseconden een update van de slang kunnen tekenen op het Canvas.

    b.  Methodes:
-   StartGame() methode die het spel klaar zet:
    -   Textblocken een gepaste inhoud geven
    -   List clearen
    -   Drie punten toevoegen aan het slangenlichaam.
    -   Start dispatcher
-   DrawGame() methode waarin er met een foreach over ieder punt van het slangenlichaam geïtereerd wordt om het te tekenen op het canvas.\
    Met snakeWindow.Children.Clear(); kan je de elementen uit het canvas
    verwijderen.\
    Met snakeWindow.Children.Add() kan je elementen toevoegen.\
    Elk onderdeel van de slang is één cel groot. Je bepaalt hier de
    grootte van ieder vakje van het scherm waar de slang kan naar toe
    bewegen. Indien de breedte en hoogte van een slangendeel vijf zijn,
    dan is iedere cel vijf op vijf.\
    \
    *Tip:*

```cs

 Ellipse snakePart = new Ellipse();

 snakePart.Fill = Brushes.LightGreen;

 snakePart.Width = \_celSize;

 snakePart.Height = \_celSize;

 Canvas.SetBottom(snakePart, positionSnakePart.Y);

 Canvas.SetLeft(snakePart, positionSnakePart.X);

 snakeWindow.Children.Add(snakePart);

```

-   In de Timer_Tick methode die gekoppeld wordt aan de DispatcherTimer
    zullen alle checks toegevoegd worden die continu moeten worden
    uitgevoerd. Momenteel willen we de DrawGame() methode hier iedere
    tick uitvoeren.

2.  Bewegen van een Slang:

    a.  Lokale Variabelen:

-   Een string variabelen (of enum) voor de huidige richting.

-   Een string variabelen (of enum) voor de nieuwe richting aan te geven
    die de volgende tick gebruik wordt. Deze kan worden aangepast door
    pijltoetsen die opgevangen worden in Window_KeyDown.

-   (Optioneel in geval van strings): Een array met de mogelijke
    richtingen.

    a.  Methodes:


-   Het Venster (Window) heeft een eventhandler
    KeyDown=\"Window_KeyDown\". Hierin kunnen we de richting van de
    slang aansturen. Gebruik de huidige richting om te berekenen wat de
    nieuwe richting zal zijn. De slang kan namelijk geen 180° draaien.

```cs

 private void Window_KeyDown(object sender, KeyEventArgs e)

 {

 switch (e.Key)

 {

 case Key.Down:

 if (\_direction != \_directions\[0\])

 {

 //Indien vorige righting niet boven was, ga naar onder

 \_direction = \_newDirection = \_directions\[2\];

 }

 break;

```

-   private Point NewPositionHead(): berekent het nieuwe Point van het
    hoofd van de slang in de volgende tick op basis van de nieuwe
    richting. In deze methode maak je gebruik van de celgrootte die je
    gebruikt hebt om de slang te tekenen.

-   Private void MoveSnake(Point newHead): verwijdert de staart uit de
    lijst van slangenlichaam en voegt het nieuwe hoofd toe aan de list.

-   Timer_Tick wordt uitgebreid met het updaten van de huidige
    richting/nieuwe richting. Verder wordt NewPositionHead() gebruikt om
    de slang te bewegen in MoveSnake(newHead) voordat de slang getekend
    wordt.

3.  Spel stoppen:

    a.  Lokale Variabelen:


a.  maxX bewaart de breedte van het canvas. Gebruik hiervoor de Width
    property van het canvas om de waarde te bekomen. Je kan deze code
    uitvoeren bij het opstarten van het spel.

b.  maxY bewaart de hoogte van het canvas.

    a.  Methodes:

-   IsGameOver(Point newHead) is een methode die controleert of het
    Point van het hoofd voor de volgende tick zich bevindt **buiten** de
    rand van **het spel** of **in** het lichaam van **de slang** .

-   In de Timer_Tick methode wordt de IsGameOver methode gebruikt om het
    spel te stoppen indien nodig. Stop de DispatcherTimer wanneer het
    spel eindigt.

4.  Genereren van eten voor de slang:

    a.  Lokale Variabelen:

-   Een lijst met coördinaten van al het eten (List\<Point\> \_food).
    Net zoals het slangenlichaam wordt deze lijst gebruikt om de
    posities voor te stellen van het eten.

-   private int \_maximumNumberOfFoodBlocks = A; om de hoeveelheid eten
    dat maximaal op het scherm mag verschijnen in te stellen.

-   private int \_celSize = B; Dit is de grootte die je gekozen hebt
    voor de grootte van de slang

    a.  Methodes:


-   Gebruik de Random klasse, \_celSize, maxX, maxY en de list van eten
    om de methode **SpawnFoodBlock** () te maken en **iedere tick** op
    te roepen om een mogelijk eetblokje toe te voegen aan de list van
    eten op een willekeurige plaats in het scherm.

-   Pas de methode DrawGame() aan om iedere tick ook het eten op het
    canvas te tekenen.

5.  Eten kunnen opeten en bijhouden van een score:

    a.  Controleer in de Timer_Tick of de positie van het nieuwe hoofd
        dezelfde positie heeft als een eetblokje in de lijst van eten.
        Indien dat zo is, verwijder het blokje uit de lijst van eten,
        maak de slang langer en verhoog de score.

6.  Afronden (hoogste score, bonussen):

    a.  Lokale Variabelen:

-   playerScore: bewaar de score van de speler om iedere keer te updaten
    in het label, wanneer er een punt wordt gescoord.

-   highScore: bewaar de hoogste score en pas deze aan, wanneer een
    speler het spel eindigt met een hogere score.

    a.  Bonussen:


-   Indien je code flexibel geschreven is, dan zou je nu met weinig werk
    een bonus kunnen implementeren die 5 punten geeft en de slang 5
    stukken langer maakt
