# 2048 klon

## Uživatelská dokumentace

2048 je posuvná logická hra, ve které kombinujete očíslované destičky tak, abyste dosáhli destičky s co nejvyšší hodnotou. Hra končí, když již není možné provést další tah.

### Ovládání

1. **Zahájení hry**: Po spuštění hry se zobrazí mřížka se dvěma dlaždicemi, které mají hodnotu `2`, nebo vzácněji `4`.

2. **Přesouvání destiček**: Pomocí kláves se šipkami nebo WASD (`Nahoru`, `Vlevo`, `Dolů`, `Vpravo`) se posouvají všechny destičky na hrací ploše zvoleným směrem až ke kraji.

3. **Spojování destiček**: Když se dvě destičky se stejnou hodnotou setkají, spojí se do jedné se svou kombinovanou hodnotou.

    - Například dvě destičky `2` se spojí na destičku `4`, dvě destičky `4` vytvoří `8` atd.

4. **Konec hry**: Hra končí, jakmile je hrací plocha plná a není již žádný tah, který by mohl kombinovat destičky. Snažíte se pokračovat ve hře co nejdéle a dosáhnout nejvyššího skóre.

5. **Znovuzahájení hry**: Chcete-li zahájit novou hru, klikněte na tlačítko `New Game`.

### Rozhraní hry

- **Hrací plocha**: Toto je herní 4x4 mřížka, na které se zobrazují destičky s čísly.

- **Skóre**: V pravém horním rohu se nachází vaše aktuální skóre. Vaše skóre se zvyšuje o spojenou hodnotu destičkek pokaždé, když jsou dvě zkombinované.

- **Tlačítko nové hry**: Kliknutím se spustí nová hra.

- **Konec hry**: Pokud již nejsou žádné možné tahy, zobrazí se na hrací ploše překryvné okno s nápisem "GAME OVER" (Hra skončila).

### Originál

- 2048 původně vytvořil [Gabriele Cirulli](https://play2048.co/).

---

## Programátorská dokumentace

### Podrobnosti souborů

#### Tile.cs

Třída `Tile` reprezentuje jednu destičku na hrací ploše, konkrétně její hodnotu a polohu. 

1. Proměnné
   - `Value`: Číselná hodnota destičky (např. 2, 4, 8, ...).
   - `HasMerged`: Boolean udávající, zda se destičky během aktuálního tahu účastnila sloučení.
   - `Row` & `Column`: Poloha na hrací ploše.
2. Metody
   - `SetMergeStatus(bool status)`: Pomocná metoda pro nastavení stavu `HasMerged` destičky.

#### Board.cs

Třída `Board` představuje herní plochu a hlavní kostru pro hru 2048. Je zodpovědná za správu stavu destiček, provádění tahů, přidávání nových destiček a určování výsledků hry.

1. Proměnné
   - `Size`: Velikost herní plochy.
   - `Score`: Aktuální skóre.
   - `Tiles`: 2D pole destiček.
   - `Random`: Instance třídy `Random` pro generování nových destiček.
   - `Direction` enum: Představuje možné směry, které lze provést (`Up`, `Down`, `Left`, `Right`).

2. Metody
   - **ResetBoard()**: Vrátí desku do počátečního stavu se dvěma náhodnými destičkami pro zahájení hry.
   - **GetTileAt()**: Vrátí objekt `Tile` na zadaném řádku a sloupci.
   - **AddRandomTile()**: Přidá novou dlaždici na náhodné volné místo na desce (10% šance na hodnotu 4).
   - **Move()**: Přesune destičky v zadaném směru.
   - **MoveLeft()** | **MoveUp()** | **MoveRight()** | **MoveDown()**: Přesune destičky do daného směru a stejně hodnotné spojí (iteruje přes řádky / sloupce a hledá, jestli jsou na sloupci / řádce vedle sebe dvě, popř. s mezerou mezi nimi, stejně hodnotné destičky).
   - **FetchTiles()** a **CombineTiles()**: Pomocné metody pro pohyb destiček, `FetchTiles` vybere nenulové destičky z řádku / sloupce a `CombineTiles` spojí stejně hodnotné destičky.
   - **HasMovesLeft()**: Zkontroluje, zda na desce zbývají platné tahy.
   - **CanMove()**: Zkontroluje, zda je možné provést tah daným směrem.

#### MainWindow.xaml

Jedná se o primární soubor pro rozvržení uživatelského rozhraní hry.

1. **Window Resources**: Obsahuje styl `NewGameButton`, který definuje vzhled a chování tlačítka "New Game".
2. **Grid Layout**: Aplikace používá k rozvržení především grid. Vnořené gridy zpracovávají herní plochu a ovládací prvky hry.
3. **GameOverOverlay**: Komponenta která leží přes hrací plochu, která se zviditelní při ukončení hry a překryje hratelné pole.

#### MainWindow.xaml.cs

Tento soubor obsahuje kód pro soubor `MainWindow.xaml`. Je zodpovědný za inicializaci a ovládání hry a aktualizaci uživatelského rozhraní.

1. **Proměnné**:
    - `Size`: Konstanta velikosti herní plochy.
    - `gameBoard`: Instance třídy `Board`, která reprezentuje aktuální stav hry.

2. **Key Events**:
    - `OnPreviewKeyDown`: Detekuje stisknutí klávesy se šipkou / kláves WASD a podle toho posouvá destičky a přidá náhodnou dlaždici, pokud je přesun platný.

3. Metody a funkce

   1. **StartGame()**: Inicializuje nebo resetuje herní plochu a skóre.
   2. **UpdateUI()**: Aktualizuje uživatelské rozhraní herní plochy tak, aby odpovídalo stavu `gameBoard`.
   3. **OnRestartButtonClick()**: Obsluha události pro tlačítko `New Game`, restartuje hru.
   4. **GetBackgroundForValue()**: Vrací barvy pozadí podle hodnot dlaždic.

#### BoardTests.cs

Tento soubor obsahuje sadu testů pro třídu `Board`. Testy jsou napsány pomocí knihovny NUnit a testují různé aspekty herní funkčnosti.

- Začíná hra ve správném počátečním stavu?
- Končí hra, když není možné provést další tah?
- Probíhají tahy správně?
- Spojují se dlaždice správně?
- Jsou některé tahy správně nepovoleny v určitých situacích?