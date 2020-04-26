using System.ComponentModel;

namespace TatlaCas.Asp.Persistence.Npgsql
{
    public enum RepoLocale
    {
        [Description("Default (Namibia)")] Default,
        [Description("Afrikaans (Namibia)")] AfNa,

        [Description("Afrikaans (South Africa)")]
        AfZa,
        [Description("Afrikaans")] Af,
        [Description("Akan (Ghana)")] AkGh,
        [Description("Akan")] Ak,
        [Description("Albanian (Albania)")] SqAl,
        [Description("Albanian")] Sq,
        [Description("Amharic (Ethiopia)")] AmEt,
        [Description("Amharic")] Am,
        [Description("Arabic (Algeria)")] ArDz,
        [Description("Arabic (Bahrain)")] ArBh,
        [Description("Arabic (Egypt)")] ArEg,
        [Description("Arabic (Iraq)")] ArIq,
        [Description("Arabic (Jordan)")] ArJo,
        [Description("Arabic (Kuwait)")] ArKw,
        [Description("Arabic (Lebanon)")] ArLb,
        [Description("Arabic (Libya)")] ArLy,
        [Description("Arabic (Morocco)")] ArMa,
        [Description("Arabic (Oman)")] ArOm,
        [Description("Arabic (Qatar)")] ArQa,
        [Description("Arabic (Saudi Arabia)")] ArSa,
        [Description("Arabic (Sudan)")] ArSd,
        [Description("Arabic (Syria)")] ArSy,
        [Description("Arabic (Tunisia)")] ArTn,

        [Description("Arabic (United Arab Emirates)")]
        ArAe,
        [Description("Arabic (Yemen)")] ArYe,
        [Description("Arabic")] Ar,
        [Description("Armenian (Armenia)")] HyAm,
        [Description("Armenian")] Hy,
        [Description("Assamese (India)")] AsIn,
        [Description("Assamese")] As,
        [Description("Asu (Tanzania)")] AsaTz,
        [Description("Asu")] Asa,

        [Description("Azerbaijani (Cyrillic)")]
        AzCyrl,

        [Description("Azerbaijani (Cyrillic, Azerbaijan)")]
        AzCyrlAz,
        [Description("Azerbaijani (Latin)")] AzLatn,

        [Description("Azerbaijani (Latin, Azerbaijan)")]
        AzLatnAz,
        [Description("Azerbaijani")] Az,
        [Description("Bambara (Mali)")] BmMl,
        [Description("Bambara")] Bm,
        [Description("Basque (Spain)")] EuEs,
        [Description("Basque")] Eu,
        [Description("Belarusian (Belarus)")] BeBy,
        [Description("Belarusian")] Be,
        [Description("Bemba (Zambia)")] BemZm,
        [Description("Bemba")] Bem,
        [Description("Bena (Tanzania)")] BezTz,
        [Description("Bena")] Bez,
        [Description("Bengali (Bangladesh)")] BnBd,
        [Description("Bengali (India)")] BnIn,
        [Description("Bengali")] Bn,

        [Description("Bosnian (Bosnia and Herzegovina)")]
        BsBa,
        [Description("Bosnian")] Bs,
        [Description("Bulgarian (Bulgaria)")] BgBg,
        [Description("Bulgarian")] Bg,

        [Description("Burmese (Myanmar [Burma])")]
        MyMm,
        [Description("Burmese")] My,

        [Description("Cantonese (Traditional, Hong Kong SAR China)")]
        YueHantHk,
        [Description("Catalan (Spain)")] CaEs,
        [Description("Catalan")] Ca,

        [Description("Central Morocco Tamazight (Latin)")]
        TzmLatn,

        [Description("Central Morocco Tamazight (Latin, Morocco)")]
        TzmLatnMa,

        [Description("Central Morocco Tamazight")]
        Tzm,

        [Description("Cherokee (United States)")]
        ChrUs,
        [Description("Cherokee")] Chr,
        [Description("Chiga (Uganda)")] CggUg,
        [Description("Chiga")] Cgg,

        [Description("Chinese (Simplified Han)")]
        ZhHans,

        [Description("Chinese (Simplified Han, China)")]
        ZhHansCn,

        [Description("Chinese (Simplified Han, Hong Kong SAR China)")]
        ZhHansHk,

        [Description("Chinese (Simplified Han, Macau SAR China)")]
        ZhHansMo,

        [Description("Chinese (Simplified Han, Singapore)")]
        ZhHansSg,

        [Description("Chinese (Traditional Han)")]
        ZhHant,

        [Description("Chinese (Traditional Han, Hong Kong SAR China)")]
        ZhHantHk,

        [Description("Chinese (Traditional Han, Macau SAR China)")]
        ZhHantMo,

        [Description("Chinese (Traditional Han, Taiwan)")]
        ZhHantTw,
        [Description("Chinese")] Zh,

        [Description("Cornish (United Kingdom)")]
        KwGb,
        [Description("Cornish")] Kw,
        [Description("Croatian (Croatia)")] HrHr,
        [Description("Croatian")] Hr,

        [Description("Czech (Czech Republic)")]
        CsCz,
        [Description("Czech")] Cs,
        [Description("Danish (Denmark)")] DaDk,
        [Description("Danish")] Da,
        [Description("Dutch (Belgium)")] NlBe,
        [Description("Dutch (Netherlands)")] NlNl,
        [Description("Dutch")] Nl,
        [Description("Embu (Kenya)")] EbuKe,
        [Description("Embu")] Ebu,

        [Description("English (American Samoa)")]
        EnAs,
        [Description("English (Australia)")] EnAu,
        [Description("English (Belgium)")] EnBe,
        [Description("English (Belize)")] EnBz,
        [Description("English (Botswana)")] EnBw,
        [Description("English (Canada)")] EnCa,
        [Description("English (Guam)")] EnGu,

        [Description("English (Hong Kong SAR China)")]
        EnHk,
        [Description("English (India)")] EnIn,
        [Description("English (Ireland)")] EnIe,
        [Description("English (Israel)")] EnIl,
        [Description("English (Jamaica)")] EnJm,
        [Description("English (Malta)")] EnMt,

        [Description("English (Marshall Islands)")]
        EnMh,
        [Description("English (Mauritius)")] EnMu,
        [Description("English (Namibia)")] EnNa,
        [Description("English (New Zealand)")] EnNz,

        [Description("English (Northern Mariana Islands)")]
        EnMp,
        [Description("English (Pakistan)")] EnPk,
        [Description("English (Philippines)")] EnPh,
        [Description("English (Singapore)")] EnSg,

        [Description("English (South Africa)")]
        EnZa,

        [Description("English (Trinidad and Tobago)")]
        EnTt,

        [Description("English (U.S. Minor Outlying Islands)")]
        EnUm,

        [Description("English (U.S. Virgin Islands)")]
        EnVi,

        [Description("English (United Kingdom)")]
        EnGb,

        [Description("English (United States)")]
        EnUs,
        [Description("English (Zimbabwe)")] EnZw,
        [Description("English")] En,
        [Description("Esperanto")] Eo,
        [Description("Estonian (Estonia)")] EtEe,
        [Description("Estonian")] Et,
        [Description("Ewe (Ghana)")] EeGh,
        [Description("Ewe (Togo)")] EeTg,
        [Description("Ewe")] Ee,

        [Description("Faroese (Faroe Islands)")]
        FoFo,
        [Description("Faroese")] Fo,

        [Description("Filipino (Philippines)")]
        FilPh,
        [Description("Filipino")] Fil,
        [Description("Finnish (Finland)")] FiFi,
        [Description("Finnish")] Fi,
        [Description("French (Belgium)")] FrBe,
        [Description("French (Benin)")] FrBj,
        [Description("French (Burkina Faso)")] FrBf,
        [Description("French (Burundi)")] FrBi,
        [Description("French (Cameroon)")] FrCm,
        [Description("French (Canada)")] FrCa,

        [Description("French (Central African Republic)")]
        FrCf,
        [Description("French (Chad)")] FrTd,
        [Description("French (Comoros)")] FrKm,

        [Description("French (Congo - Brazzaville)")]
        FrCg,

        [Description("French (Congo - Kinshasa)")]
        FrCd,

        [Description("French (Côte d’Ivoire)")]
        FrCi,
        [Description("French (Djibouti)")] FrDj,

        [Description("French (Equatorial Guinea)")]
        FrGq,
        [Description("French (France)")] FrFr,
        [Description("French (Gabon)")] FrGa,
        [Description("French (Guadeloupe)")] FrGp,
        [Description("French (Guinea)")] FrGn,
        [Description("French (Luxembourg)")] FrLu,
        [Description("French (Madagascar)")] FrMg,
        [Description("French (Mali)")] FrMl,
        [Description("French (Martinique)")] FrMq,
        [Description("French (Monaco)")] FrMc,
        [Description("French (Niger)")] FrNe,
        [Description("French (Rwanda)")] FrRw,
        [Description("French (Réunion)")] FrRe,

        [Description("French (Saint Barthélemy)")]
        FrBl,
        [Description("French (Saint Martin)")] FrMf,
        [Description("French (Senegal)")] FrSn,
        [Description("French (Switzerland)")] FrCh,
        [Description("French (Togo)")] FrTg,
        [Description("French")] Fr,
        [Description("Fulah (Senegal)")] FfSn,
        [Description("Fulah")] Ff,
        [Description("Galician (Spain)")] GlEs,
        [Description("Galician")] Gl,
        [Description("Ganda (Uganda)")] LgUg,
        [Description("Ganda")] Lg,
        [Description("Georgian (Georgia)")] KaGe,
        [Description("Georgian")] Ka,
        [Description("German (Austria)")] DeAt,
        [Description("German (Belgium)")] DeBe,
        [Description("German (Germany)")] DeDe,

        [Description("German (Liechtenstein)")]
        DeLi,
        [Description("German (Luxembourg)")] DeLu,
        [Description("German (Switzerland)")] DeCh,
        [Description("German")] De,
        [Description("Greek (Cyprus)")] ElCy,
        [Description("Greek (Greece)")] ElGr,
        [Description("Greek")] El,
        [Description("Gujarati (India)")] GuIn,
        [Description("Gujarati")] Gu,
        [Description("Gusii (Kenya)")] GuzKe,
        [Description("Gusii")] Guz,
        [Description("Hausa (Latin)")] HaLatn,
        [Description("Hausa (Latin, Ghana)")] HaLatnGh,
        [Description("Hausa (Latin, Niger)")] HaLatnNe,

        [Description("Hausa (Latin, Nigeria)")]
        HaLatnNg,
        [Description("Hausa")] Ha,

        [Description("Hawaiian (United States)")]
        HawUs,
        [Description("Hawaiian")] Haw,
        [Description("Hebrew (Israel)")] HeIl,
        [Description("Hebrew")] He,
        [Description("Hindi (India)")] HiIn,
        [Description("Hindi")] Hi,
        [Description("Hungarian (Hungary)")] HuHu,
        [Description("Hungarian")] Hu,
        [Description("Icelandic (Iceland)")] IsIs,
        [Description("Icelandic")] Is,
        [Description("Igbo (Nigeria)")] IgNg,
        [Description("Igbo")] Ig,

        [Description("Indonesian (Indonesia)")]
        IdId,
        [Description("Indonesian")] Id,
        [Description("Irish (Ireland)")] GaIe,
        [Description("Irish")] Ga,
        [Description("Italian (Italy)")] ItIt,
        [Description("Italian (Switzerland)")] ItCh,
        [Description("Italian")] It,
        [Description("Japanese (Japan)")] JaJp,
        [Description("Japanese")] Ja,

        [Description("Kabuverdianu (Cape Verde)")]
        KeaCv,
        [Description("Kabuverdianu")] Kea,
        [Description("Kabyle (Algeria)")] KabDz,
        [Description("Kabyle")] Kab,

        [Description("Kalaallisut (Greenland)")]
        KlGl,
        [Description("Kalaallisut")] Kl,
        [Description("Kalenjin (Kenya)")] KlnKe,
        [Description("Kalenjin")] Kln,
        [Description("Kamba (Kenya)")] KamKe,
        [Description("Kamba")] Kam,
        [Description("Kannada (India)")] KnIn,
        [Description("Kannada")] Kn,
        [Description("Kazakh (Cyrillic)")] KkCyrl,

        [Description("Kazakh (Cyrillic, Kazakhstan)")]
        KkCyrlKz,
        [Description("Kazakh")] Kk,
        [Description("Khmer (Cambodia)")] KmKh,
        [Description("Khmer")] Km,
        [Description("Kikuyu (Kenya)")] KiKe,
        [Description("Kikuyu")] Ki,
        [Description("Kinyarwanda (Rwanda)")] RwRw,
        [Description("Kinyarwanda")] Rw,
        [Description("Konkani (India)")] KokIn,
        [Description("Konkani")] Kok,
        [Description("Korean (South Korea)")] KoKr,
        [Description("Korean")] Ko,
        [Description("Koyra Chiini (Mali)")] KhqMl,
        [Description("Koyra Chiini")] Khq,

        [Description("Koyraboro Senni (Mali)")]
        SesMl,
        [Description("Koyraboro Senni")] Ses,
        [Description("Langi (Tanzania)")] LagTz,
        [Description("Langi")] Lag,
        [Description("Latvian (Latvia)")] LvLv,
        [Description("Latvian")] Lv,

        [Description("Lithuanian (Lithuania)")]
        LtLt,
        [Description("Lithuanian")] Lt,
        [Description("Luo (Kenya)")] LuoKe,
        [Description("Luo")] Luo,
        [Description("Luyia (Kenya)")] LuyKe,
        [Description("Luyia")] Luy,

        [Description("Macedonian (Macedonia)")]
        MkMk,
        [Description("Macedonian")] Mk,
        [Description("Machame (Tanzania)")] JmcTz,
        [Description("Machame")] Jmc,
        [Description("Makonde (Tanzania)")] KdeTz,
        [Description("Makonde")] Kde,
        [Description("Malagasy (Madagascar)")] MgMg,
        [Description("Malagasy")] Mg,
        [Description("Malay (Brunei)")] MsBn,
        [Description("Malay (Malaysia)")] MsMy,
        [Description("Malay")] Ms,
        [Description("Malayalam (India)")] MlIn,
        [Description("Malayalam")] Ml,
        [Description("Maltese (Malta)")] MtMt,
        [Description("Maltese")] Mt,
        [Description("Manx (United Kingdom)")] GvGb,
        [Description("Manx")] Gv,
        [Description("Marathi (India)")] MrIn,
        [Description("Marathi")] Mr,
        [Description("Masai (Kenya)")] MasKe,
        [Description("Masai (Tanzania)")] MasTz,
        [Description("Masai")] Mas,
        [Description("Meru (Kenya)")] MerKe,
        [Description("Meru")] Mer,
        [Description("Morisyen (Mauritius)")] MfeMu,
        [Description("Morisyen")] Mfe,
        [Description("Nama (Namibia)")] NaqNa,
        [Description("Nama")] Naq,
        [Description("Nepali (India)")] NeIn,
        [Description("Nepali (Nepal)")] NeNp,
        [Description("Nepali")] Ne,

        [Description("North Ndebele (Zimbabwe)")]
        NdZw,
        [Description("North Ndebele")] Nd,

        [Description("Norwegian Bokmål (Norway)")]
        NbNo,
        [Description("Norwegian Bokmål")] Nb,

        [Description("Norwegian Nynorsk (Norway)")]
        NnNo,
        [Description("Norwegian Nynorsk")] Nn,
        [Description("Nyankole (Uganda)")] NynUg,
        [Description("Nyankole")] Nyn,
        [Description("Oriya (India)")] OrIn,
        [Description("Oriya")] Or,
        [Description("Oromo (Ethiopia)")] OmEt,
        [Description("Oromo (Kenya)")] OmKe,
        [Description("Oromo")] Om,
        [Description("Pashto (Afghanistan)")] PsAf,
        [Description("Pashto")] Ps,
        [Description("Persian (Afghanistan)")] FaAf,
        [Description("Persian (Iran)")] FaIr,
        [Description("Persian")] Fa,
        [Description("Polish (Poland)")] PlPl,
        [Description("Polish")] Pl,
        [Description("Portuguese (Brazil)")] PtBr,

        [Description("Portuguese (Guinea-Bissau)")]
        PtGw,

        [Description("Portuguese (Mozambique)")]
        PtMz,
        [Description("Portuguese (Portugal)")] PtPt,
        [Description("Portuguese")] Pt,
        [Description("Punjabi (Arabic)")] PaArab,

        [Description("Punjabi (Arabic, Pakistan)")]
        PaArabPk,
        [Description("Punjabi (Gurmukhi)")] PaGuru,

        [Description("Punjabi (Gurmukhi, India)")]
        PaGuruIn,
        [Description("Punjabi")] Pa,
        [Description("Romanian (Moldova)")] RoMd,
        [Description("Romanian (Romania)")] RoRo,
        [Description("Romanian")] Ro,
        [Description("Romansh (Switzerland)")] RmCh,
        [Description("Romansh")] Rm,
        [Description("Rombo (Tanzania)")] RofTz,
        [Description("Rombo")] Rof,
        [Description("Russian (Moldova)")] RuMd,
        [Description("Russian (Russia)")] RuRu,
        [Description("Russian (Ukraine)")] RuUa,
        [Description("Russian")] Ru,
        [Description("Rwa (Tanzania)")] RwkTz,
        [Description("Rwa")] Rwk,
        [Description("Samburu (Kenya)")] SaqKe,
        [Description("Samburu")] Saq,

        [Description("Sango (Central African Republic)")]
        SgCf,
        [Description("Sango")] Sg,
        [Description("Sena (Mozambique)")] SehMz,
        [Description("Sena")] Seh,
        [Description("Serbian (Cyrillic)")] SrCyrl,

        [Description("Serbian (Cyrillic, Bosnia and Herzegovina)")]
        SrCyrlBa,

        [Description("Serbian (Cyrillic, Montenegro)")]
        SrCyrlMe,

        [Description("Serbian (Cyrillic, Serbia)")]
        SrCyrlRs,
        [Description("Serbian (Latin)")] SrLatn,

        [Description("Serbian (Latin, Bosnia and Herzegovina)")]
        SrLatnBa,

        [Description("Serbian (Latin, Montenegro)")]
        SrLatnMe,

        [Description("Serbian (Latin, Serbia)")]
        SrLatnRs,
        [Description("Serbian")] Sr,
        [Description("Shona (Zimbabwe)")] SnZw,
        [Description("Shona")] Sn,
        [Description("Sichuan Yi (China)")] IiCn,
        [Description("Sichuan Yi")] Ii,
        [Description("Sinhala (Sri Lanka)")] SiLk,
        [Description("Sinhala")] Si,
        [Description("Slovak (Slovakia)")] SkSk,
        [Description("Slovak")] Sk,
        [Description("Slovenian (Slovenia)")] SlSi,
        [Description("Slovenian")] Sl,
        [Description("Soga (Uganda)")] XogUg,
        [Description("Soga")] Xog,
        [Description("Somali (Djibouti)")] SoDj,
        [Description("Somali (Ethiopia)")] SoEt,
        [Description("Somali (Kenya)")] SoKe,
        [Description("Somali (Somalia)")] SoSo,
        [Description("Somali")] So,
        [Description("Spanish (Argentina)")] EsAr,
        [Description("Spanish (Bolivia)")] EsBo,
        [Description("Spanish (Chile)")] EsCl,
        [Description("Spanish (Colombia)")] EsCo,
        [Description("Spanish (Costa Rica)")] EsCr,

        [Description("Spanish (Dominican Republic)")]
        EsDo,
        [Description("Spanish (Ecuador)")] EsEc,
        [Description("Spanish (El Salvador)")] EsSv,

        [Description("Spanish (Equatorial Guinea)")]
        EsGq,
        [Description("Spanish (Guatemala)")] EsGt,
        [Description("Spanish (Honduras)")] EsHn,

        [Description("Spanish (Latin America)")]
        Es419,
        [Description("Spanish (Mexico)")] EsMx,
        [Description("Spanish (Nicaragua)")] EsNi,
        [Description("Spanish (Panama)")] EsPa,
        [Description("Spanish (Paraguay)")] EsPy,
        [Description("Spanish (Peru)")] EsPe,
        [Description("Spanish (Puerto Rico)")] EsPr,
        [Description("Spanish (Spain)")] EsEs,

        [Description("Spanish (United States)")]
        EsUs,
        [Description("Spanish (Uruguay)")] EsUy,
        [Description("Spanish (Venezuela)")] EsVe,
        [Description("Spanish")] Es,
        [Description("Swahili (Kenya)")] SwKe,
        [Description("Swahili (Tanzania)")] SwTz,
        [Description("Swahili")] Sw,
        [Description("Swedish (Finland)")] SvFi,
        [Description("Swedish (Sweden)")] SvSe,
        [Description("Swedish")] Sv,

        [Description("Swiss German (Switzerland)")]
        GswCh,
        [Description("Swiss German")] Gsw,
        [Description("Tachelhit (Latin)")] ShiLatn,

        [Description("Tachelhit (Latin, Morocco)")]
        ShiLatnMa,
        [Description("Tachelhit (Tifinagh)")] ShiTfng,

        [Description("Tachelhit (Tifinagh, Morocco)")]
        ShiTfngMa,
        [Description("Tachelhit")] Shi,
        [Description("Taita (Kenya)")] DavKe,
        [Description("Taita")] Dav,
        [Description("Tamil (India)")] TaIn,
        [Description("Tamil (Sri Lanka)")] TaLk,
        [Description("Tamil")] Ta,
        [Description("Telugu (India)")] TeIn,
        [Description("Telugu")] Te,
        [Description("Teso (Kenya)")] TeoKe,
        [Description("Teso (Uganda)")] TeoUg,
        [Description("Teso")] Teo,
        [Description("Thai (Thailand)")] ThTh,
        [Description("Thai")] Th,
        [Description("Tibetan (China)")] BoCn,
        [Description("Tibetan (India)")] BoIn,
        [Description("Tibetan")] Bo,
        [Description("Tigrinya (Eritrea)")] TiEr,
        [Description("Tigrinya (Ethiopia)")] TiEt,
        [Description("Tigrinya")] Ti,
        [Description("Tonga (Tonga)")] ToTo,
        [Description("Tonga")] To,
        [Description("Turkish (Turkey)")] TrTr,
        [Description("Turkish")] Tr,
        [Description("Ukrainian (Ukraine)")] UkUa,
        [Description("Ukrainian")] Uk,
        [Description("Urdu (India)")] UrIn,
        [Description("Urdu (Pakistan)")] UrPk,
        [Description("Urdu")] Ur,
        [Description("Uzbek (Arabic)")] UzArab,

        [Description("Uzbek (Arabic, Afghanistan)")]
        UzArabAf,
        [Description("Uzbek (Cyrillic)")] UzCyrl,

        [Description("Uzbek (Cyrillic, Uzbekistan)")]
        UzCyrlUz,
        [Description("Uzbek (Latin)")] UzLatn,

        [Description("Uzbek (Latin, Uzbekistan)")]
        UzLatnUz,
        [Description("Uzbek")] Uz,
        [Description("Vietnamese (Vietnam)")] ViVn,
        [Description("Vietnamese")] Vi,
        [Description("Vunjo (Tanzania)")] VunTz,
        [Description("Vunjo")] Vun,

        [Description("Welsh (United Kingdom)")]
        CyGb,
        [Description("Welsh")] Cy,
        [Description("Yoruba (Nigeria)")] YoNg,
        [Description("Yoruba")] Yo,
        [Description("Zulu (South Africa)")] ZuZa,
        [Description("Zulu")] Zu,
    }
}