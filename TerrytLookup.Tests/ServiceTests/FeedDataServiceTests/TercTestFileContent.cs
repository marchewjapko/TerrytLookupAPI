namespace TerrytLookup.Tests.ServiceTests.FeedDataServiceTests;

public static class TercTestFileContent
{
    public const string Content = """
                                  WOJ;POW;GMI;RODZ;NAZWA;NAZWA_DOD;STAN_NA
                                  02;;;;DOLNOŚLĄSKIE;województwo;2024-01-01
                                  02;01;;;bolesławiecki;powiat;2024-01-01
                                  02;01;01;1;Bolesławiec;gmina miejska;2024-01-01
                                  02;01;02;2;Bolesławiec;gmina wiejska;2024-01-01
                                  02;01;03;2;Gromadka;gmina wiejska;2024-01-01
                                  02;01;04;3;Nowogrodziec;gmina miejsko-wiejska;2024-01-01
                                  02;01;04;4;Nowogrodziec;miasto;2024-01-01
                                  02;01;04;5;Nowogrodziec;obszar wiejski;2024-01-01
                                  02;01;05;2;Osiecznica;gmina wiejska;2024-01-01
                                  02;01;06;2;Warta Bolesławiecka;gmina wiejska;2024-01-01
                                  02;02;;;dzierżoniowski;powiat;2024-01-01
                                  02;02;01;1;Bielawa;gmina miejska;2024-01-01
                                  02;02;02;1;Dzierżoniów;gmina miejska;2024-01-01
                                  02;02;03;3;Pieszyce;gmina miejsko-wiejska;2024-01-01
                                  02;02;03;4;Pieszyce;miasto;2024-01-01
                                  02;02;03;5;Pieszyce;obszar wiejski;2024-01-01
                                  02;02;04;1;Piława Górna;gmina miejska;2024-01-01
                                  02;02;05;2;Dzierżoniów;gmina wiejska;2024-01-01
                                  02;02;06;2;Łagiewniki;gmina wiejska;2024-01-01
                                  02;02;07;3;Niemcza;gmina miejsko-wiejska;2024-01-01
                                  02;02;07;4;Niemcza;miasto;2024-01-01
                                  02;02;07;5;Niemcza;obszar wiejski;2024-01-01
                                  02;03;;;głogowski;powiat;2024-01-01
                                  02;03;01;1;Głogów;gmina miejska;2024-01-01
                                  02;03;02;2;Głogów;gmina wiejska;2024-01-01
                                  02;03;03;2;Jerzmanowa;gmina wiejska;2024-01-01
                                  02;03;04;2;Kotla;gmina wiejska;2024-01-01
                                  02;03;05;2;Pęcław;gmina wiejska;2024-01-01
                                  02;03;06;2;Żukowice;gmina wiejska;2024-01-01
                                  02;04;;;górowski;powiat;2024-01-01
                                  02;04;01;3;Góra;gmina miejsko-wiejska;2024-01-01
                                  02;04;01;4;Góra;miasto;2024-01-01
                                  02;04;01;5;Góra;obszar wiejski;2024-01-01
                                  02;04;02;2;Jemielno;gmina wiejska;2024-01-01
                                  02;04;03;2;Niechlów;gmina wiejska;2024-01-01
                                  02;04;04;3;Wąsosz;gmina miejsko-wiejska;2024-01-01
                                  02;04;04;4;Wąsosz;miasto;2024-01-01
                                  02;04;04;5;Wąsosz;obszar wiejski;2024-01-01
                                  02;05;;;jaworski;powiat;2024-01-01
                                  02;05;01;1;Jawor;gmina miejska;2024-01-01
                                  02;05;02;3;Bolków;gmina miejsko-wiejska;2024-01-01
                                  02;05;02;4;Bolków;miasto;2024-01-01
                                  02;05;02;5;Bolków;obszar wiejski;2024-01-01
                                  02;05;03;2;Męcinka;gmina wiejska;2024-01-01
                                  02;05;04;2;Mściwojów;gmina wiejska;2024-01-01
                                  02;05;05;2;Paszowice;gmina wiejska;2024-01-01
                                  02;05;06;2;Wądroże Wielkie;gmina wiejska;2024-01-01
                                  02;06;;;karkonoski;powiat;2024-01-01
                                  02;06;01;1;Karpacz;gmina miejska;2024-01-01
                                  02;06;02;1;Kowary;gmina miejska;2024-01-01
                                  02;06;03;1;Piechowice;gmina miejska;2024-01-01
                                  02;06;04;1;Szklarska Poręba;gmina miejska;2024-01-01
                                  02;06;05;2;Janowice Wielkie;gmina wiejska;2024-01-01
                                  02;06;06;2;Jeżów Sudecki;gmina wiejska;2024-01-01
                                  02;06;07;2;Mysłakowice;gmina wiejska;2024-01-01
                                  02;06;08;2;Podgórzyn;gmina wiejska;2024-01-01
                                  02;06;09;2;Stara Kamienica;gmina wiejska;2024-01-01
                                  02;07;;;kamiennogórski;powiat;2024-01-01
                                  02;07;01;1;Kamienna Góra;gmina miejska;2024-01-01
                                  02;07;02;2;Kamienna Góra;gmina wiejska;2024-01-01
                                  02;07;03;3;Lubawka;gmina miejsko-wiejska;2024-01-01
                                  02;07;03;4;Lubawka;miasto;2024-01-01
                                  02;07;03;5;Lubawka;obszar wiejski;2024-01-01
                                  02;07;04;2;Marciszów;gmina wiejska;2024-01-01
                                  02;08;;;kłodzki;powiat;2024-01-01
                                  02;08;01;1;Duszniki-Zdrój;gmina miejska;2024-01-01
                                  02;08;02;1;Kłodzko;gmina miejska;2024-01-01
                                  02;08;03;1;Kudowa-Zdrój;gmina miejska;2024-01-01
                                  02;08;04;1;Nowa Ruda;gmina miejska;2024-01-01
                                  02;08;05;1;Polanica-Zdrój;gmina miejska;2024-01-01
                                  02;08;06;3;Bystrzyca Kłodzka;gmina miejsko-wiejska;2024-01-01
                                  02;08;06;4;Bystrzyca Kłodzka;miasto;2024-01-01
                                  02;08;06;5;Bystrzyca Kłodzka;obszar wiejski;2024-01-01
                                  02;08;07;2;Kłodzko;gmina wiejska;2024-01-01
                                  02;08;08;3;Lądek-Zdrój;gmina miejsko-wiejska;2024-01-01
                                  02;08;08;4;Lądek-Zdrój;miasto;2024-01-01
                                  02;08;08;5;Lądek-Zdrój;obszar wiejski;2024-01-01
                                  02;08;09;2;Lewin Kłodzki;gmina wiejska;2024-01-01
                                  02;08;10;3;Międzylesie;gmina miejsko-wiejska;2024-01-01
                                  02;08;10;4;Międzylesie;miasto;2024-01-01
                                  02;08;10;5;Międzylesie;obszar wiejski;2024-01-01
                                  02;08;11;2;Nowa Ruda;gmina wiejska;2024-01-01
                                  02;08;12;3;Radków;gmina miejsko-wiejska;2024-01-01
                                  02;08;12;4;Radków;miasto;2024-01-01
                                  02;08;12;5;Radków;obszar wiejski;2024-01-01
                                  02;08;13;3;Stronie Śląskie;gmina miejsko-wiejska;2024-01-01
                                  02;08;13;4;Stronie Śląskie;miasto;2024-01-01
                                  02;08;13;5;Stronie Śląskie;obszar wiejski;2024-01-01
                                  02;08;14;3;Szczytna;gmina miejsko-wiejska;2024-01-01
                                  02;08;14;4;Szczytna;miasto;2024-01-01
                                  02;08;14;5;Szczytna;obszar wiejski;2024-01-01
                                  02;09;;;legnicki;powiat;2024-01-01
                                  02;09;01;1;Chojnów;gmina miejska;2024-01-01
                                  02;09;02;2;Chojnów;gmina wiejska;2024-01-01
                                  02;09;03;2;Krotoszyce;gmina wiejska;2024-01-01
                                  02;09;04;2;Kunice;gmina wiejska;2024-01-01
                                  02;09;05;2;Legnickie Pole;gmina wiejska;2024-01-01
                                  02;09;06;2;Miłkowice;gmina wiejska;2024-01-01
                                  02;09;07;3;Prochowice;gmina miejsko-wiejska;2024-01-01
                                  02;09;07;4;Prochowice;miasto;2024-01-01
                                  """;
}