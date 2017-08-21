# VRP
<p class="alert alert-info">
    Program stworzony w ramach pracy magisterskiej. Rozwiązuje on problem marszrutyzacji przy pomocy dwóch technik heurystycznych: algorytmu genetycznego oraz algorytmu Clarke and Wright.
    Program szuka najkrótszej trasy łączącej wszystkich klientów oraz zaczynającej się i kończącej się w magazynie.
    Adresy (nazwy ulic i numery) magazynu i klientów znajdują się w Rzeszowie i podawane są przez użytkownika.
    Program napisano w języku C#, z wykorzystaniem technologii ASP.NET opartej na wzorcu Model-View-Controller (MVC).<br />
    Czas implementacji programu: ~ 44 h.
</p>

<hr />

<h4>Znajdź trasę</h4>
W zakładce „Znajdź trasę” mieszczą się pola tekstowe, do których należy wpisać adresy magazynu oraz klientów.
Pola te wbudowany mają mechanim sugerowania. Dla ulic włącza się on po wpisaniu trzech znaków, natomiast dla numerów po jednym.
Domyślnie wyświetlone są pola dla trzech lokalizacji, jednak można dodać ich dowolną liczbę korzystając z przycisku „Dodaj klienta”.
Po wyznaczeniu trasy na samym dole na mapie pojawią się dwie ścieżki oraz znaczniki, jeden w kolorze niebieskim, a pozostałe w czerwonym, które symbolizują odpowiednio magazyn oraz klientów.
Po kliknięciu na dowolny z nich, pojawi się jego adres. Trasa oznaczona kolorem ciemnozielonym została wyznaczona za pomocą algorytmu genetycznego,
natomiast ścieżka jasnozielona z wykorzystaniem algorytmu Clarke and Wright. Dodatkowo tuż nad mapą znajdują się długości (w kilometrach) owych tras.

<hr />
<h4>Test algorytmów</h4>
Test polega na zsumowaniu 50 wyników uzyskanych przez algorytm gentyczny dla pewnych ustawień algorytmu, w celu wybrania tych najlepszych. Wyniki te zapisywane są w pliku excel.
Jedna kolumna, to suma 50 wyników dla jednego ustawienia. i-ty wiersz to suma 50 wyników uzyskanych w i-tym kroku algorytmu.
Najlpeszy wynik spośród wszystkich wywołań algorytmu genetycznego wyświetlany jest na mapie wraz z wynikiem uzystkanym przez algorytm Clarke and Wright, a także podany jest czas
wykonań obu algorytmów.<br /><br />

Zaimplementowane algorytmy przetestowano na sześciu zbiorach testowych składających się z 10, 20 i 40 lokalizacji, znajdujących się w miastach Rzeszów i Warszawa.<br /><br />
<p><b>Test #1</b> przeprowadzony został dla następujących 20 lokalizacji, znajdujących się w Rzeszowie (pierwszy adres, to magazyn): <br /></p>
<p>
    Lucjana Siemieńskiego 14, Szpitalna 5, Eugeniusza Kwiatkowskiego 2,
    Ignacego Solarza 19a, Aleja Powstańców Warszawy 20, Jagiellońska 5, Obrońców Poczty Gdańskiej 30, Ofiar Katynia 15, Wierzbowa 27, Aleja Józefa Piłsudskiego 34, Zygmuntowska 8,
    Witolda 8, Aleja Tadeusza Rejtana 51, Hetmańska 56, Aleja Tadeusza Rejtana 49, Strażacka 27, Jana Kasprowicza 2, Przemysłowa 3, Podkarpacka 8a oraz Leska 2.<br />
</p>

<p>
    <b>Test #2</b> przeprowadzony został dla następujących 40 lokalizacji, znajdujących się w Rzeszowie (pierwszy adres, to magazyn):
</p>
<p>
    Świętego Rocha 24, Ignacego Paderewskiego 69,
    Stanisława Mikołajczyka 31, Miła 9b, Ossolińskich 61, Konfederatów Barskich 40, Krokusowa 7c, Zygmunta I Starego 12A/3, Ustrzycka 114a, Stanisława Witkacego 1, Morgowa 138,
    Słocińska 141, Dębicka 288, Sasanki 55, Miodowa 47, Mieszka I 92, Miła 2a, Jarowa 36, Władysława Warneńczyka 81, Tarnowska 13,Dębicka 4a, Jana Brzechwy 12,
    Aleja generała Władysława Sikorskiego 223, Ignacego Paderewskiego 3f, Wiśniowa 10, Aleksandra Zelwerowicza 70, Słocińska 88c, Forsycji 8/1,
    Kornela Makuszyńskiego 6, Zawiszy Czarnego 14, Miła 10, Ignacego Solarza 18/29, Macieja Rataja 16/2, Hrabiego Alfreda Potockiego 131, Zwięczycka 8, Świętego Marcina 20A,
    Zawilcowa 1/6, Długa 7, Irysowa 6oraz Jasna 29.
</p>
<p>
    <b>Test #3</b> przeprowadzony został dla następujących 10 lokalizacji, znajdujących się w Rzeszowie (pierwszy adres, to magazyn):
</p>
<p>
    Skrajna 12, Jana Styki 15, Ciepłownicza 1a,
    Aleja Tadeusza Rejtana 2, 8 Marca 6, Podwisłocze 33, Fryderyka Chopina 35a, Eugeniusza Kwiatkowskiego 52b, Jarowa 206 oraz Konfederatów Barskich 63.
</p>
<p>
    <b>Test #4</b> przeprowadzony został dla następujących 20 lokalizacji, znajdujących się w Warszawie (pierwszy adres, to magazyn):
</p>
<p>
    Aleje Jerozolimskie 56C, Grzybowska 61, Chmielna 36,
    Obozowa 16, Aleja "Solidarności" 115, Deotymy 56, Dzika 2, Solec 24, Międzynarodowa 68, Grochowska 338/340, Terespolska 4, Generała Romana Abrahama 1, Jana III Sobieskiego 18,
    Jagiellońska 82, Kijowska 20, Bartycka 183, Złotej Wilgi 2, Jana Olbrachta 46, Piotra Wysockiego 10 oraz Pruszkowska 4D.
</p>
<p>
    <b>Test #5</b> przeprowadzony został dla następujących 40 lokalizacji, znajdujących się w Warszawie (pierwszy adres, to magazyn):
</p>
<p>
    Wał Miedzeszyński 19, Nowoczesna 9, Okopowa 55A,
    Mikołaja Gomółki 16, Kasztanowa 33, Nadwiślańska 24, Leona Berensona 83, Kazimierzowska 2, Henryka Arctowskiego 17, Bogumiła Zuga 32A, Augustyna Kordeckiego 11, Wojskowa 40a,
    Cmentarna 24, Marszałkowska 107,Zapłocie 258, Wojciecha Oczki 4B, Knyszyńska 7, Nadwiślańska 270, Mariana Pisarka 9, Aleje Jerozolimskie 244, Szklarniowa 88, Radłowa 30A, Powsińska 31,
    Ezopa 46a, Zamiejska 15, Mieszka I 47, Głubczycka 34, Wojciecha Korfantego 63, Księdza Wacława Kuleszy10, Masłowiecka 1B, Bratkowa 4, Lawendowa 16, Stroma 18, Pszczyńska 20a, Krucza 49,
    Zaułek 25, Włókiennicza 46, Grochowska 365A, Gorzelnicza 9a oraz Jana Pawła II 202
</p>
<p>
    <b>Test #6</b> przeprowadzony został dla następujących 10 lokalizacji, znajdujących się w Warszawie (pierwszy adres, to magazyn):
</p>
<p>
    Łazienkowska 6A, Wał Miedzeszyński 510, Wał Zawadowski 225,
    Bartycka 18A, Wał Miedzeszyński 377, Wioślarska 6, Łotewska 9A, Karowa 2, Wybrzeże Szczecińskie 6orazWybrzeże Kościuszkowskie 20.
</p>
