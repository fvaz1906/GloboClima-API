namespace GloboClima.Domain.Entities
{
    public class CountryData : BaseEntity
    {
        public CommonName Name { get; set; }
        public string[] Tld { get; set; } // Domínios de topo
        public string Cca2 { get; set; } // Código de país de 2 letras
        public string Cca3 { get; set; } // Código de país de 3 letras
        public string Cioc { get; set; } // Código IOC do país
        public bool Independent { get; set; } // Se o país é independente
        public string Status { get; set; } // Status do país
        public bool UnMember { get; set; } // Se é membro da ONU
        public Dictionary<string, Currency> Currencies { get; set; } // Moedas
        public string[] Capital { get; set; } // Capitais
        public string Region { get; set; } // Região (ex: Americas)
        public string Subregion { get; set; } // Sub-região
        public Dictionary<string, string> Languages { get; set; } // Línguas
        public double[] Latlng { get; set; } // Coordenadas de latitude e longitude
        public bool Landlocked { get; set; } // Se o país é sem litoral
        public string[] Borders { get; set; } // Países fronteiriços
        public double Area { get; set; } // Área do país
        public Demonyms Demonyms { get; set; } // Gentílicos
        public string Flag { get; set; } // Bandeira (emoji)
        public Maps Maps { get; set; } // URLs de mapas
        public long Population { get; set; } // População
        public Gini Gini { get; set; } // Índice de Gini
        public string Fifa { get; set; } // Código FIFA
        public string[] Timezones { get; set; } // Fusos horários
        public string[] Continents { get; set; } // Continente
        public Flags Flags { get; set; } // URLs das bandeiras
        public CoatOfArms CoatOfArms { get; set; } // Brasão de armas
        public PostalCode PostalCode { get; set; } // Informações de código postal
    }
}
