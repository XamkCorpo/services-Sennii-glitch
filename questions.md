# Vaihe 3: Service-kerros, Repository, Result Pattern ja API-dokumentaatio — Teoriakysymykset

Vastaa alla oleviin kysymyksiin omin sanoin. Kirjoita vastauksesi kysymysten alle.

> **Vinkki:** Jos jokin kysymys tuntuu vaikealta, palaa lukemaan teoriamateriaalit:
> - [Service-kerros ja DI](https://github.com/xamk-mire/Xamk-wiki/blob/main/C%23/fin/04-Advanced/WebAPI/Services-and-DI.md)
> - [Repository Pattern](https://github.com/xamk-mire/Xamk-wiki/blob/main/C%23/fin/04-Advanced/Patterns/Repository-Pattern.md)
> - [Result Pattern](https://github.com/xamk-mire/Xamk-wiki/blob/main/C%23/fin/04-Advanced/Patterns/Result-Pattern.md)

---

## Osa 1: Service-kerros

### Kysymys 1: Fat Controller -ongelma

Miksi on ongelma jos controller sisältää kaiken logiikan (tietokantakyselyt, muunnokset, validoinnin)? Anna vähintään kaksi konkreettista haittaa.

**Vastaus:**

Testaaminen ja muokkaaminen vaikeutuu, controller kasvaa isoksi ja sama logiikka tulee moneen paikkaan.
---

### Kysymys 2: Vastuunjako

Miten vastuut jakautuvat controller:n, service:n ja repository:n välillä tässä harjoituksessa? Kirjoita lyhyt kuvaus kunkin kerroksen tehtävästä.

**Controller vastaa:** 
http-kerros

**Service vastaa:**
liiketoiminta logiikka 

**Repository vastaa:**
tietokantakoodi

---

### Kysymys 3: DTO-muunnokset servicessä

Miksi DTO ↔ Entity -muunnokset kuuluvat serviceen eikä controlleriin? Mitä hyötyä siitä on, että controller ei tunne `Product`-entiteettiä lainkaan?

**Vastaus:**
Rakenne pysyy selkeänä ja kerrokset erillään. Kun controller keskittyy vain http-asioihin siitä tulee helpommin testattava ja selkeämpi. 

---

## Osa 2: Interface ja Dependency Injection

### Kysymys 4: Interface vs. konkreettinen luokka

Miksi controller injektoi `IProductService`-interfacen eikä suoraan `ProductService`-luokkaa? Mitä hyötyä tästä on?

**Vastaus:**

luokka ei luo itse tarvitsemiaan riippuvukksia, näin testaaminen on mahdollista.
---

### Kysymys 5: DI-elinkaaret

Selitä ero näiden kolmen elinkaaren välillä ja anna esimerkki milloin kutakin käytetään:

- **AddScoped:** yksi http-pyyntö, käytetään kun dbcontext
- **AddSingleton:** sovelluksen koko elinkaari, konfiguraatio, välimuisti, yhteyspoolit
- **AddTransient:** luodaan uusi joka kerta, kevyet, tilattomat apuluokat

Miksi `AddScoped` on oikea valinta `ProductService`:lle?
käyttää dbcontextia, käytetään yleensä oletuksena

---

### Kysymys 6: DI-kontti

Selitä omin sanoin mitä DI-kontti tekee kun HTTP-pyyntö saapuu ja `ProductsController` tarvitsee `IProductService`:ä. Mitä tapahtuu vaihe vaiheelta?

**Vastaus:**
-huomaa että konstruktorissa pyydetään IProductService
-katsoo rekisteröinnin
-luo ProductServicen ja sen riippuvuudet

---

### Kysymys 7: Rekisteröinnin unohtaminen

Mitä tapahtuu jos unohdat rekisteröidä `IProductService`:n `Program.cs`:ssä? Milloin virhe ilmenee ja miltä se näyttää?

**Vastaus:**
Sovellus kaatuu kun se ajetaan, InvalidOperationException: Unable to resolve service for type 'IProductService'

---

## Osa 3: Repository-kerros

### Kysymys 8: Miksi repository?

`ProductService` käytti aluksi `AppDbContext`:ia suoraan. Miksi se refaktoroitiin käyttämään `IProductRepository`:a? Anna vähintään kaksi syytä.

**Vastaus:**
se refactoroitiin jotta olisi loose coupling ja testatavuus paranisi

---

### Kysymys 9: Service vs. Repository

Mikä on `IProductService`:n ja `IProductRepository`:n välinen ero? Mitä tietotyyppejä kumpikin käsittelee (DTO vai Entity)?

**IProductService:** käsittelee DTO:ta ja liiketoimintalogiikkaa

**IProductRepository:** käsittelee Entityjä ja tietokantatoimintoja


---

### Kysymys 10: Controllerin muuttumattomuus

Kun Vaihe 7:ssä lisättiin repository-kerros, `ProductsController` ei muuttunut lainkaan. Miksi? Mitä tämä kertoo rajapintojen (interface) hyödystä?

**Vastaus:**
se ei muuttunut, koska se oli jo riippuvainen vain IProductService‑rajapinnasta, ei sen toteutuksesta

---

## Osa 4: Exception-käsittely ja lokitus

### Kysymys 11: ILogger

Mikä on `ILogger` ja miksi sitä tarvitaan? Mistä lokit näkee kehitysympäristössä?

**Vastaus:**
ASP.NET Coren sisäänrakennettu lokitusrajapinta, siihen kirjataan mitä tapahtui ja milloin. Ne tulostuvat konsoliin kun ohjelman ajaa.

---

### Kysymys 12: Odotetut vs. odottamattomat virheet

Selitä ero "odotetun" ja "odottamattoman" virheen välillä. Anna esimerkki kummastakin ja kerro miten ne käsitellään eri tavalla servicessä.

**Odotettu virhe (esimerkki + käsittely):**  Odotettu virhe on sellainen jonka voi estää käsittelemällä itse kuten tuotetta ei löydy, validointi epäonnistuu

**Odottamaton virhe (esimerkki + käsittely):** Esim. tietokanta alhaalla tai yhteys katkennut, _logger.LogError + throw;


---

## Osa 5: Result Pattern

### Kysymys 13: Miksi null ja bool eivät riitä?

Alla on kaksi esimerkkiä. Selitä miksi ensimmäinen tapa on ongelmallinen ja miten toinen ratkaisee ongelman:

```csharp
// Tapa 1: null
ProductResponse? product = await _service.GetByIdAsync(id);
if (product == null)
    return NotFound();

// Tapa 2: Result
Result<ProductResponse> result = await _service.GetByIdAsync(id);
if (result.IsFailure)
    return NotFound(new { error = result.Error });
```

**Vastaus:**
Ensimmäinen ei kerro miksi epäonnistui, toinen kertoo mikä meni pieleen

---

### Kysymys 14: Result.Success vs. Result.Failure

Miten `Result Pattern` muutti virheiden käsittelyä servicessä? Vertaa Vaihe 8:n `throw;`-tapaa Vaihe 9:n `Result.Failure`-tapaan: mitä eroa niillä on asiakkaan (API:n kutsuja) näkökulmasta?

**Vastaus:**
Vaihe 9 mahdollistaa oikeat virhekoodit

---

## Osa 6: API-dokumentaatio

### Kysymys 15: IActionResult vs. ActionResult\<T\>

Miksi `ActionResult<ProductResponse>` on parempi kuin `IActionResult`? Anna vähintään kaksi syytä.

**Vastaus:**
kääntäjä tietää mitä endpoint palauttaa ja Swagger pystyy generoimaan response-scheman automaattisesti

---

### Kysymys 16: ProducesResponseType

Mitä `[ProducesResponseType]`-attribuutti tekee? Miten se näkyy Swagger UI:ssa?

**Vastaus:**
Dokumentoi kaikki mahdolliset statuskoodit jokaiselle endpointille, Swagger näyttää oikeat response-tyypit jokaiselle statuskoodille

---

### Kysymys 18: Refaktorointi

Sovelluksen toiminnallisuus pysyi täysin samana koko harjoituksen ajan — samat endpointit, samat vastaukset. Mitä refaktorointi tarkoittaa ja miksi se kannattaa, vaikka käyttäjä ei huomaa eroa?

**Vastaus:**
Refaktoroinnissa rakennetta parannetaan. Koodia on hellpo ylläpitää ja laajentaa ja testattavuus paranee.

---
