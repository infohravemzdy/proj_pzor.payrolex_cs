using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HraveMzdy.Procezor.Payrolex.Generator
{
    public abstract class ImportData
    {
        public void ExportField(StreamWriter writer, string field)
        {
            writer.Write(field);
            writer.Write("|");
        }
        public string ExportString(string field)
        {
            return $"{field}|";
        }

        public abstract void ExportToExportFile(StreamWriter writer);
        public abstract string Export();
    }
    public class ImportData01 : ImportData
    {
        public string IMP_OSC;
        public string IMP01_RODCIS;
        public string IMP01_RODPRIJ;
        public string IMP01_UTVAR;
        public string IMP01_STRED;
        public string IMP01_CINN;
        public string IMP01_ZAK;
        public string IMP01_PRIJ;
        public string IMP01_JMENO;
        public string IMP01_TITULPRED;
        public string IMP01_TITULZA;
        public string IMP01_DALSIPRIJ;
        public string IMP01_DATUMNAR;
        public string IMP01_MISTONAR;
        public string IMP01_RODSTAV;
        public string IMP01_ZAPOCETROKY;
        public string IMP01_ZAPOCETDNY;
        public string IMP01_ZAPOCETDATE;
        public string IMP_ADRESA_OBEC;
        public string IMP_ADRESA_COBEC;
        public string IMP_ADRESA_DCIS;
        public string IMP_ADRESA_DTYP;
        public string IMP_ADRESA_ULICE;
        public string IMP_ADRESA_PSC;
        public string IMP_ADRESA_POSTA;
        public string IMP_ADRESA_OCIS;
        public string IMP_ADRESA_STAT;
        public string IMP01_POCETDETI;
        public string IMP01_DUCHDATUM;
        public string IMP01_DUCHDRUH;
        public string IMP01_ZDRAVSTAV;
        public string IMP01_OBCANSTVI;
        public string IMP01_PRUKAZOBCAN;
        public string IMP01_PRUKAZPAS;
        public string IMP01_RODCIS_JINE;
        public string IMP01_STATNAR;
        public string IMP01_ZDROJ;

        public ImportData01()
        {
            IMP_OSC = "";
            IMP01_RODCIS = "";
            IMP01_RODPRIJ = "#";
            IMP01_UTVAR = "Útvar1";
            IMP01_STRED = "#";
            IMP01_CINN = "#";
            IMP01_ZAK = "#";
            IMP01_PRIJ = "";
            IMP01_JMENO = "";
            IMP01_TITULPRED = "#";
            IMP01_TITULZA = "#";
            IMP01_DALSIPRIJ = "#";
            IMP01_DATUMNAR = "";
            IMP01_MISTONAR = "#";
            IMP01_RODSTAV = "#";
            IMP01_ZAPOCETROKY = "#";
            IMP01_ZAPOCETDNY = "#";
            IMP01_ZAPOCETDATE = "#";
            IMP_ADRESA_OBEC = "#";
            IMP_ADRESA_COBEC = "#";
            IMP_ADRESA_DCIS = "#";
            IMP_ADRESA_DTYP = "#";
            IMP_ADRESA_ULICE = "#";
            IMP_ADRESA_PSC = "#";
            IMP_ADRESA_POSTA = "#";
            IMP_ADRESA_OCIS = "#";
            IMP_ADRESA_STAT = "#";
            IMP01_POCETDETI = "#";
            IMP01_DUCHDATUM = "#";
            IMP01_DUCHDRUH = "#";
            IMP01_ZDRAVSTAV = "#";
            IMP01_OBCANSTVI = "#";
            IMP01_PRUKAZOBCAN = "#";
            IMP01_PRUKAZPAS = "#";
            IMP01_RODCIS_JINE = "#";
            IMP01_STATNAR = "#";
            IMP01_ZDROJ = "#";
        }

        public override string Export()
        {
            if (IMP01_RODCIS.Length < 10)
            {
                IMP01_RODCIS = IMP01_RODCIS.PadRight(10, '0');
            }
            string strRodneCislo = IMP01_RODCIS;
            if (IMP01_DATUMNAR.Length > 0)
            {
                strRodneCislo = IMP01_DATUMNAR + "::" + IMP01_RODCIS;
            }
            if (string.IsNullOrWhiteSpace(IMP01_RODCIS))
            {
                strRodneCislo = "nemá rodné číslo";
            }

            StringBuilder b = new StringBuilder("@1||");
            b.Append(ExportString(IMP_OSC));
            b.Append(ExportString(strRodneCislo));
            b.Append(ExportString(IMP01_RODPRIJ));
            b.Append(ExportString(IMP01_UTVAR));
            b.Append(ExportString(IMP01_STRED));
            b.Append(ExportString(IMP01_CINN));
            b.Append(ExportString(IMP01_ZAK));
            b.Append(ExportString(IMP01_PRIJ));
            b.Append(ExportString(IMP01_JMENO));
            b.Append(ExportString(IMP01_TITULPRED));
            b.Append(ExportString(IMP01_TITULZA));
            b.Append(ExportString(IMP01_DALSIPRIJ));
            b.Append(ExportString(IMP01_MISTONAR));
            b.Append(ExportString(RODSTAV()));
            b.Append(ExportString(IMP01_ZAPOCETROKY));
            b.Append(ExportString(IMP01_ZAPOCETDNY));
            b.Append(ExportString(IMP01_ZAPOCETDATE));
            b.Append(ExportString(IMP_ADRESA_OBEC));
            b.Append(ExportString(IMP_ADRESA_COBEC));
            b.Append(ExportString(IMP_ADRESA_DCIS));
            b.Append(ExportString(IMP_ADRESA_DTYP));
            b.Append(ExportString(IMP_ADRESA_ULICE));
            b.Append(ExportString(IMP_ADRESA_PSC));
            b.Append(ExportString(IMP_ADRESA_POSTA));
            b.Append(ExportString(IMP_ADRESA_OCIS));
            b.Append(ExportString(IMP_ADRESA_STAT));
            b.Append(ExportString(IMP01_POCETDETI));
            b.Append(ExportString(IMP01_DUCHDATUM));
            b.Append(ExportString(DUCHOD()));
            b.Append(ExportString(IMP01_ZDRAVSTAV));
            b.Append(ExportString(IMP01_OBCANSTVI));
            b.Append(ExportString(IMP01_PRUKAZOBCAN));
            b.Append(ExportString(IMP01_PRUKAZPAS));
            b.Append(ExportString(IMP01_RODCIS_JINE));
            b.Append(ExportString(IMP01_STATNAR));
            b.Append(ExportString(IMP01_ZDROJ));
            return b.ToString();
        }
        public override void ExportToExportFile(StreamWriter writer)
        {
            if (string.IsNullOrEmpty(IMP_OSC))
            {
                return;
            }
            if (string.IsNullOrEmpty(IMP01_PRIJ))
            {
                return;
            }
            if (string.IsNullOrEmpty(IMP01_JMENO))
            {
                return;
            }
            writer.Write(Export());
            writer.WriteLine();
        }
        public string RODSTAV()
        {
            string strReturn = "0";
            switch (IMP01_RODSTAV)
            {
                case "Svobodný/svobodná":
                    strReturn = "1";
                    break;
                case "Ženatý/vdaná":
                    strReturn = "2";
                    break;
                case "Vdovec/vdova":
                    strReturn = "4";
                    break;
                case "Rozvedený/rozvedená":
                    strReturn = "3";
                    break;
                case "Druh/družka":
                    strReturn = "5";
                    break;
                case "Registrované partnerství":
                    strReturn = "6";
                    break;
            }
            return strReturn;
        }
        public string DUCHOD()
        {
            string strReturn = "0";
            switch (IMP01_DUCHDRUH)
            {
                case "Nepobírá důchod":
                    strReturn = "0";
                    break;
                case "Starobní důchod":
                    strReturn = "1";
                    break;
                case "Invalidní 1.nebo 2.stupně":
                    strReturn = "8";
                    break;
                case "Invalidní 3.stupně":
                    strReturn = "2";
                    break;
            }
            return strReturn;
        }
    }
    public class ImportData02 : ImportData
    {
        public string IMP_OSC;
        public string IMP02_ZPUSOB;
        public string IMP02_OBEC;
        public string IMP02_ADRESA_COBEC;
        public string IMP02_ADRESA_DCIS;
        public string IMP02_ADRESA_DTYP;
        public string IMP02_ADRESA_ULICE;
        public string IMP02_ADRESA_PSC;
        public string IMP02_ADRESA_POSTA;
        public string IMP02_ADRESA_OCIS;
        public string IMP_ADRESA_STAT;
        public string IMP02_UCET;
        public string IMP_BKSPOJ_USTAV;
        public string IMP_BKSPOJ_KSYMB;
        public string IMP_BKSPOJ_VSYMB;
        public string IMP_BKSPOJ_SSYMB;

        public ImportData02()
        {
            IMP_OSC = "";
            IMP02_ZPUSOB = "";
            IMP02_OBEC = "#";
            IMP02_ADRESA_COBEC = "#";
            IMP02_ADRESA_DCIS = "#";
            IMP02_ADRESA_DTYP = "#";
            IMP02_ADRESA_ULICE = "#";
            IMP02_ADRESA_PSC = "#";
            IMP02_ADRESA_POSTA = "#";
            IMP02_ADRESA_OCIS = "#";
            IMP_ADRESA_STAT = "#";
            IMP02_UCET = "";
            IMP_BKSPOJ_USTAV = "#";
            IMP_BKSPOJ_KSYMB = "#";
            IMP_BKSPOJ_VSYMB = "#";
            IMP_BKSPOJ_SSYMB = "#";
        }
        public override string Export()
        {
            StringBuilder b = new StringBuilder("@2||");
            b.Append(ExportString(IMP_OSC));
            b.Append(ExportString(IMP02_ZPUSOB));
            b.Append(ExportString(IMP02_OBEC));
            b.Append(ExportString(IMP02_ADRESA_COBEC));
            b.Append(ExportString(IMP02_ADRESA_DCIS));
            b.Append(ExportString(IMP02_ADRESA_DTYP));
            b.Append(ExportString(IMP02_ADRESA_ULICE));
            b.Append(ExportString(IMP02_ADRESA_PSC));
            b.Append(ExportString(IMP02_ADRESA_POSTA));
            b.Append(ExportString(IMP02_ADRESA_OCIS));
            b.Append(ExportString(IMP_ADRESA_STAT));
            b.Append(ExportString(IMP02_UCET));
            b.Append(ExportString(IMP_BKSPOJ_USTAV));
            b.Append(ExportString(IMP_BKSPOJ_KSYMB));
            b.Append(ExportString(IMP_BKSPOJ_VSYMB));
            b.Append(ExportString(IMP_BKSPOJ_SSYMB));

            return b.ToString();
        }
        public override void ExportToExportFile(StreamWriter writer)
        {
            if (string.IsNullOrEmpty(IMP_OSC))
            {
                return;
            }
            writer.Write("@2||");
            ExportField(writer, IMP_OSC);
            ExportField(writer, IMP02_ZPUSOB);
            ExportField(writer, IMP02_OBEC);
            ExportField(writer, IMP02_ADRESA_COBEC);
            ExportField(writer, IMP02_ADRESA_DCIS);
            ExportField(writer, IMP02_ADRESA_DTYP);
            ExportField(writer, IMP02_ADRESA_ULICE);
            ExportField(writer, IMP02_ADRESA_PSC);
            ExportField(writer, IMP02_ADRESA_POSTA);
            ExportField(writer, IMP02_ADRESA_OCIS);
            ExportField(writer, IMP_ADRESA_STAT);
            ExportField(writer, IMP02_UCET);
            ExportField(writer, IMP_BKSPOJ_USTAV);
            ExportField(writer, IMP_BKSPOJ_KSYMB);
            ExportField(writer, IMP_BKSPOJ_VSYMB);
            ExportField(writer, IMP_BKSPOJ_SSYMB);

            writer.WriteLine();
        }
    }
    public class ImportData05 : ImportData
    {
        public string IMP_OSC;
        public string IMP05_KODZDRAVPOJ;
        public string IMP05_MISTOZDRAVP;
        public string IMP05_CISLOPOJIST;
        public string IMP05_ZPOJCIZINEC;

        public ImportData05()
        {
            IMP_OSC = "";
            IMP05_KODZDRAVPOJ = "000";
            IMP05_MISTOZDRAVP = "#";
            IMP05_CISLOPOJIST = "#";
            IMP05_ZPOJCIZINEC = "#";
        }
        public override string Export()
        {
            StringBuilder b = new StringBuilder("@5||");
            b.Append(ExportString(IMP_OSC));
            b.Append(ExportString(IMP05_KODZDRAVPOJ));
            b.Append(ExportString(IMP05_MISTOZDRAVP));
            b.Append(ExportString(IMP05_CISLOPOJIST));
            b.Append(ExportString(IMP05_ZPOJCIZINEC));

            return b.ToString();
        }
        public override void ExportToExportFile(StreamWriter writer)
        {
            if (string.IsNullOrEmpty(IMP_OSC))
            {
                return;
            }
            writer.Write("@5||");
            ExportField(writer, IMP_OSC);
            ExportField(writer, IMP05_KODZDRAVPOJ);
            ExportField(writer, IMP05_MISTOZDRAVP);
            ExportField(writer, IMP05_CISLOPOJIST);
            ExportField(writer, IMP05_ZPOJCIZINEC);

            writer.WriteLine();

        }
    }
    public class ImportData07 : ImportData
    {
        public string IMP_OSC;
        public string IMP_ROK;
        public string IMP07_PRIJMYZAOBD;
        public string IMP07_PODEPSAL;
        public string IMP07_INVALIDITA1;
        public string IMP07_INVALIDITA2;
        public string IMP07_INVALIDITA3;
        public string IMP07_PRIJEM;
        public string IMP07_POJISTNE;
        public string IMP07_ZALOHA;
        public string IMP07_DANZUCTOVAT;
        public string IMP07_SLEVAB;
        public string IMP07_SLEVAC;
        public string IMP07_BONUSC;
        public string IMP07_PENZPPR;
        public string IMP07_KZIVPPR;
        public string IMP07_REZIDENT;
        public string IMP07_SOLPRIJEM;
        public string IMP07_REZTAXID;
        public string IMP07_REZTYPID;

        public ImportData07()
        {
            IMP_OSC = "";
            IMP_ROK = "";
            IMP07_PRIJMYZAOBD = "#";
            IMP07_PODEPSAL = "#";
            IMP07_INVALIDITA1 = "#";
            IMP07_INVALIDITA2 = "#";
            IMP07_INVALIDITA3 = "#";
            IMP07_PRIJEM = "#";
            IMP07_POJISTNE = "#";
            IMP07_ZALOHA = "#";
            IMP07_DANZUCTOVAT = "#";
            IMP07_SLEVAB = "#";
            IMP07_SLEVAC = "#";
            IMP07_BONUSC = "#";
            IMP07_PENZPPR = "#";
            IMP07_KZIVPPR = "#";
            IMP07_REZIDENT = "#";
            IMP07_SOLPRIJEM = "#";
            IMP07_REZTAXID = "#";
            IMP07_REZTYPID = "#";
        }
        public override string Export()
        {
            StringBuilder b = new StringBuilder("@7||");
            b.Append(ExportString(IMP_OSC));
            b.Append(ExportString(IMP_ROK));
            b.Append(ExportString(IMP07_PRIJMYZAOBD));
            b.Append(ExportString(IMP07_PODEPSAL));
            b.Append(ExportString(IMP07_INVALIDITA1));
            b.Append(ExportString(IMP07_INVALIDITA2));
            b.Append(ExportString(IMP07_INVALIDITA3));
            b.Append(ExportString(IMP07_PRIJEM));
            b.Append(ExportString(IMP07_POJISTNE));
            b.Append(ExportString(IMP07_ZALOHA));
            b.Append(ExportString(IMP07_DANZUCTOVAT));
            b.Append(ExportString(IMP07_SLEVAB));
            b.Append(ExportString(IMP07_SLEVAC));
            b.Append(ExportString(IMP07_BONUSC));
            b.Append(ExportString(IMP07_PENZPPR));
            b.Append(ExportString(IMP07_KZIVPPR));
            b.Append(ExportString(IMP07_REZIDENT));
            b.Append(ExportString(IMP07_SOLPRIJEM));
            b.Append(ExportString(IMP07_REZTAXID));
            b.Append(ExportString(IMP07_REZTYPID));

            return b.ToString();
        }
        public override void ExportToExportFile(StreamWriter writer)
        {
            if (string.IsNullOrEmpty(IMP_OSC))
            {
                return;
            }
            writer.Write("@7||");
            ExportField(writer, IMP_OSC);
            ExportField(writer, IMP_ROK);
            ExportField(writer, IMP07_PRIJMYZAOBD);
            ExportField(writer, IMP07_PODEPSAL);
            ExportField(writer, IMP07_INVALIDITA1);
            ExportField(writer, IMP07_INVALIDITA2);
            ExportField(writer, IMP07_INVALIDITA3);
            ExportField(writer, IMP07_PRIJEM);
            ExportField(writer, IMP07_POJISTNE);
            ExportField(writer, IMP07_ZALOHA);
            ExportField(writer, IMP07_DANZUCTOVAT);
            ExportField(writer, IMP07_SLEVAB);
            ExportField(writer, IMP07_SLEVAC);
            ExportField(writer, IMP07_BONUSC);
            ExportField(writer, IMP07_PENZPPR);
            ExportField(writer, IMP07_KZIVPPR);
            ExportField(writer, IMP07_REZIDENT);
            ExportField(writer, IMP07_SOLPRIJEM);
            ExportField(writer, IMP07_REZTAXID);
            ExportField(writer, IMP07_REZTYPID);

            writer.WriteLine();

        }
    }
    public class ImportData08 : ImportData
    {
        public string IMP_OSC;
        public string IMP08_PODEPSAL;
        public string IMP08_INVALIDITA1;
        public string IMP08_INVALIDITA2;
        public string IMP08_INVALIDITA3;
        public string IMP08_REZIDENT;

        public ImportData08()
        {
            IMP_OSC = "";
            IMP08_PODEPSAL = "1";
            IMP08_INVALIDITA1 = "0";
            IMP08_INVALIDITA2 = "0";
            IMP08_INVALIDITA3 = "0";
            IMP08_REZIDENT = "#";
        }
        public override string Export()
        {
            StringBuilder b = new StringBuilder("@8||");
            b.Append(ExportString(IMP_OSC));
            b.Append(ExportString(IMP08_PODEPSAL));
            b.Append(ExportString(IMP08_INVALIDITA1));
            b.Append(ExportString(IMP08_INVALIDITA2));
            b.Append(ExportString(IMP08_INVALIDITA3));
            b.Append(ExportString(IMP08_REZIDENT));

            return b.ToString();
        }
        public override void ExportToExportFile(StreamWriter writer)
        {
            if (string.IsNullOrEmpty(IMP_OSC))
            {
                return;
            }
            writer.Write("@8||");
            ExportField(writer, IMP_OSC);
            ExportField(writer, IMP08_PODEPSAL);
            ExportField(writer, IMP08_INVALIDITA1);
            ExportField(writer, IMP08_INVALIDITA2);
            ExportField(writer, IMP08_INVALIDITA3);
            ExportField(writer, IMP08_REZIDENT);

            writer.WriteLine();

        }
    }
    public class ImportData10 : ImportData
    {
        public string IMP_OSC;
        public string IMP_ROK;
        public string IMP10_SKOLA;
        public string IMP10_KONUPLATMES;
        public string IMP10_KONUPLATROK;
        public string IMP10_PLATNOSTOBD;
        public string IMP10_AKTUALNIOBD;
        public string IMP10_ICO;
        public string IMP10_SKOLMEST;
        public string IMP10_SKOLULIC;
        public string IMP10_SKOLPPSC;

        public ImportData10()
        {
            IMP_OSC = "";
            IMP_ROK = "";
            IMP10_SKOLA = "SLEVA STUDENT";
            IMP10_KONUPLATMES = "12";
            IMP10_KONUPLATROK = "2021";
            IMP10_PLATNOSTOBD = "#";
            IMP10_AKTUALNIOBD = "0";
            IMP10_ICO = "SKOLA_IC";
            IMP10_SKOLMEST = "#";
            IMP10_SKOLULIC = "#";
            IMP10_SKOLPPSC = "#";
        }
        public override string Export()
        {
            StringBuilder b = new StringBuilder("@10||");
            b.Append(ExportString(IMP_OSC));
            b.Append(ExportString(IMP_ROK));
            b.Append(ExportString(IMP10_SKOLA));
            b.Append(ExportString(IMP10_KONUPLATMES));
            b.Append(ExportString(IMP10_KONUPLATROK));
            b.Append(ExportString(IMP10_PLATNOSTOBD));
            b.Append(ExportString(IMP10_AKTUALNIOBD));
            b.Append(ExportString(IMP10_ICO));
            b.Append(ExportString(IMP10_SKOLMEST));
            b.Append(ExportString(IMP10_SKOLULIC));
            b.Append(ExportString(IMP10_SKOLPPSC));

            return b.ToString();
        }
        public override void ExportToExportFile(StreamWriter writer)
        {
            if (string.IsNullOrEmpty(IMP_OSC))
            {
                return;
            }
            writer.Write("@10||");
            ExportField(writer, IMP_OSC);
            ExportField(writer, IMP_ROK);
            ExportField(writer, IMP10_SKOLA);
            ExportField(writer, IMP10_KONUPLATMES);
            ExportField(writer, IMP10_KONUPLATROK);
            ExportField(writer, IMP10_PLATNOSTOBD);
            ExportField(writer, IMP10_AKTUALNIOBD);
            ExportField(writer, IMP10_ICO);
            ExportField(writer, IMP10_SKOLMEST);
            ExportField(writer, IMP10_SKOLULIC);
            ExportField(writer, IMP10_SKOLPPSC);

            writer.WriteLine();

        }
    }
    public class ImportData09 : ImportData
    {
        public string IMP_OSC;
        public string IMP_ROK;
        public string IMP09_DATUMNAR;
        public string IMP09_RODCIS;
        public string IMP09_PRIJ;
        public string IMP09_JMENO;
        public string IMP09_TITULPRED;
        public string IMP09_TITULZA;
        public string IMP09_ZTPP;
        public string IMP09_KONUPLATMES;
        public string IMP09_KONUPLATROK;
        public string IMP09_PLATNOSTOBD;
        public string IMP09_AKTUALNIOBD;
        public string IMP09_PLATNOSTPOR;
        public string IMP09_AKTUALNIPOR;

        public ImportData09()
        {
            IMP_OSC = "";
            IMP_ROK = "";
            IMP09_DATUMNAR = "#";
            IMP09_RODCIS = "#";
            IMP09_PRIJ = "#";
            IMP09_JMENO = "#";
            IMP09_TITULPRED = "#";
            IMP09_TITULZA = "#";
            IMP09_ZTPP = "#";
            IMP09_KONUPLATMES = "#";
            IMP09_KONUPLATROK = "#";
            IMP09_PLATNOSTOBD = "#";
            IMP09_AKTUALNIOBD = "#";
            IMP09_PLATNOSTPOR = "#";
            IMP09_AKTUALNIPOR = "#";
        }
        public override string Export()
        {
            if (IMP09_RODCIS.Length < 10)
            {
                IMP09_RODCIS = IMP09_RODCIS.PadRight(10, '0');
            }
            string strRodneCislo = IMP09_RODCIS;
            if (IMP09_DATUMNAR.Length > 0)
            {
                strRodneCislo = IMP09_DATUMNAR + "::" + IMP09_RODCIS;
            }
            if (string.IsNullOrWhiteSpace(IMP09_RODCIS))
            {
                strRodneCislo = "nemá rodné číslo";
            }

            StringBuilder b = new StringBuilder("@9||");
            b.Append(ExportString(IMP_OSC));
            b.Append(ExportString(IMP_ROK));
            b.Append(ExportString(strRodneCislo));
            b.Append(ExportString(IMP09_PRIJ));
            b.Append(ExportString(IMP09_JMENO));
            b.Append(ExportString(IMP09_TITULPRED));
            b.Append(ExportString(IMP09_TITULZA));
            b.Append(ExportString(IMP09_ZTPP));
            b.Append(ExportString(IMP09_KONUPLATMES));
            b.Append(ExportString(IMP09_KONUPLATROK));
            b.Append(ExportString(IMP09_PLATNOSTOBD));
            b.Append(ExportString(IMP09_AKTUALNIOBD));
            b.Append(ExportString(IMP09_PLATNOSTPOR));
            b.Append(ExportString(IMP09_AKTUALNIPOR));

            return b.ToString();
        }
        public override void ExportToExportFile(StreamWriter writer)
        {
            if (IMP09_RODCIS.Length < 10)
            {
                IMP09_RODCIS = IMP09_RODCIS.PadRight(10, '0');
            }
            string strRodneCislo = IMP09_RODCIS;
            if (IMP09_DATUMNAR.Length > 0)
            {
                strRodneCislo = IMP09_DATUMNAR + "::" + IMP09_RODCIS;
            }
            if (string.IsNullOrWhiteSpace(IMP09_RODCIS))
            {
                strRodneCislo = "nemá rodné číslo";
            }

            if (string.IsNullOrEmpty(IMP_OSC))
            {
                return;
            }
            writer.Write("@9||");
            ExportField(writer, IMP_OSC);
            ExportField(writer, IMP_ROK);
            ExportField(writer, strRodneCislo);
            ExportField(writer, IMP09_PRIJ);
            ExportField(writer, IMP09_JMENO);
            ExportField(writer, IMP09_TITULPRED);
            ExportField(writer, IMP09_TITULZA);
            ExportField(writer, IMP09_ZTPP);
            ExportField(writer, IMP09_KONUPLATMES);
            ExportField(writer, IMP09_KONUPLATROK);
            ExportField(writer, IMP09_PLATNOSTOBD);
            ExportField(writer, IMP09_AKTUALNIOBD);
            ExportField(writer, IMP09_PLATNOSTPOR);
            ExportField(writer, IMP09_AKTUALNIPOR);

            writer.WriteLine();

        }
    }
    public class ImportData32 : ImportData
    {
        public string IMP_OSC;
        public string IMP_ROK;
        public string IMP32_RODCIS;
        public string IMP32_DATUMNAR;
        public string IMP32_PRIJ;
        public string IMP32_JMENO;
        public string IMP32_TITULPRED;
        public string IMP32_TITULZA;

        public ImportData32()
        {
            IMP_OSC = "";
            IMP_ROK = "";
            IMP32_RODCIS = "#";
            IMP32_DATUMNAR = "#";
            IMP32_PRIJ = "#";
            IMP32_JMENO = "#";
            IMP32_TITULPRED = "#";
            IMP32_TITULZA = "#";
        }
        public override string Export()
        {
            if (IMP32_RODCIS.Length < 10)
            {
                IMP32_RODCIS = IMP32_RODCIS.PadRight(10, '0');
            }
            string strRodneCislo = IMP32_RODCIS;
            if (IMP32_DATUMNAR.Length > 0)
            {
                strRodneCislo = IMP32_DATUMNAR + "::" + IMP32_RODCIS;
            }
            if (string.IsNullOrWhiteSpace(IMP32_RODCIS))
            {
                strRodneCislo = "nemá rodné číslo";
            }

            StringBuilder b = new StringBuilder("@32||");
            b.Append(ExportString(IMP_OSC));
            b.Append(ExportString(IMP_ROK));
            b.Append(ExportString(strRodneCislo));
            b.Append(ExportString(IMP32_PRIJ));
            b.Append(ExportString(IMP32_JMENO));
            b.Append(ExportString(IMP32_TITULPRED));
            b.Append(ExportString(IMP32_TITULZA));

            return b.ToString();
        }
        public override void ExportToExportFile(StreamWriter writer)
        {
            if (IMP32_RODCIS.Length < 10)
            {
                IMP32_RODCIS = IMP32_RODCIS.PadRight(10, '0');
            }
            string strRodneCislo = IMP32_RODCIS;
            if (IMP32_DATUMNAR.Length > 0)
            {
                strRodneCislo = IMP32_DATUMNAR + "::" + IMP32_RODCIS;
            }
            if (string.IsNullOrWhiteSpace(IMP32_RODCIS))
            {
                strRodneCislo = "nemá rodné číslo";
            }
            if (string.IsNullOrEmpty(IMP_OSC))
            {
                return;
            }
            writer.Write("@32||");
            ExportField(writer, IMP_OSC);
            ExportField(writer, IMP_ROK);
            ExportField(writer, strRodneCislo);
            ExportField(writer, IMP32_PRIJ);
            ExportField(writer, IMP32_JMENO);
            ExportField(writer, IMP32_TITULPRED);
            ExportField(writer, IMP32_TITULZA);

            writer.WriteLine();

        }
    }
    public class ImportData31 : ImportData
    {
        public string IMP_OSC;
        public string IMP_ADRESA_OBEC;
        public string IMP_ADRESA_COBEC;
        public string IMP_ADRESA_DCIS;
        public string IMP_ADRESA_DTYP;
        public string IMP_ADRESA_ULICE;
        public string IMP_ADRESA_PSC;
        public string IMP_ADRESA_POSTA;
        public string IMP_ADRESA_OCIS;
        public string IMP_ADRESA_STAT;

        public ImportData31()
        {
            IMP_OSC = "";
            IMP_ADRESA_OBEC = "";
            IMP_ADRESA_COBEC = "#";
            IMP_ADRESA_DCIS = "#";
            IMP_ADRESA_DTYP = "#";
            IMP_ADRESA_ULICE = "#";
            IMP_ADRESA_PSC = "#";
            IMP_ADRESA_POSTA = "#";
            IMP_ADRESA_OCIS = "#";
            IMP_ADRESA_STAT = "#";
        }
        public override string Export()
        {
            StringBuilder b = new StringBuilder("@31||");
            b.Append(ExportString(IMP_OSC));
            b.Append(ExportString(IMP_ADRESA_OBEC));
            b.Append(ExportString(IMP_ADRESA_COBEC));
            b.Append(ExportString(IMP_ADRESA_DCIS));
            b.Append(ExportString(IMP_ADRESA_DTYP));
            b.Append(ExportString(IMP_ADRESA_ULICE));
            b.Append(ExportString(IMP_ADRESA_PSC));
            b.Append(ExportString(IMP_ADRESA_POSTA));
            b.Append(ExportString(IMP_ADRESA_OCIS));
            b.Append(ExportString(IMP_ADRESA_STAT));

            return b.ToString();
        }
        public override void ExportToExportFile(StreamWriter writer)
        {
            if (string.IsNullOrEmpty(IMP_OSC))
            {
                return;
            }
            if (string.IsNullOrEmpty(IMP_ADRESA_OBEC))
            {
                return;
            }
            writer.Write("@31||");
            ExportField(writer, IMP_OSC);
            ExportField(writer, IMP_ADRESA_OBEC);
            ExportField(writer, IMP_ADRESA_COBEC);
            ExportField(writer, IMP_ADRESA_DCIS);
            ExportField(writer, IMP_ADRESA_DTYP);
            ExportField(writer, IMP_ADRESA_ULICE);
            ExportField(writer, IMP_ADRESA_PSC);
            ExportField(writer, IMP_ADRESA_POSTA);
            ExportField(writer, IMP_ADRESA_OCIS);
            ExportField(writer, IMP_ADRESA_STAT);

            writer.WriteLine();

        }
    }
    public class ImportData03 : ImportData
    {
        public string IMP_OSC;
        public string IMP03_VZDELNEJ;
        public string IMP03_VZDELOBOR;
        public string IMP03_JKOV;
        public string IMP03_RIDIC;
        public string IMP03_TEL1CIS;
        public string IMP03_TEL1TYP;
        public string IMP03_TEL2CIS;
        public string IMP03_TEL2TYP;
        public string IMP03_VOJAK;
        public string IMP03_OSDATA;
        public string IMP03_PREDCHOZI;
        public string IMP03_POZNAMKA;
        public string IMP03_PRACMAIL;
        public string IMP03_PDFHESLO;
        public string IMP03_CERTFILE;
        public string IMP03_VZDELISPV;
        public string IMP03_VZDEL_QRY;
        public string IMP03_VZDEL_WWW;

        public ImportData03()
        {
            IMP_OSC = "";
            IMP03_VZDELNEJ = "#";
            IMP03_VZDELOBOR = "#";
            IMP03_JKOV = "#";
            IMP03_RIDIC = "#";
            IMP03_TEL1CIS = "#";
            IMP03_TEL1TYP = "#";
            IMP03_TEL2CIS = "#";
            IMP03_TEL2TYP = "#";
            IMP03_VOJAK = "#";
            IMP03_OSDATA = "#";
            IMP03_PREDCHOZI = "#";
            IMP03_POZNAMKA = "#";
            IMP03_PRACMAIL = "#";
            IMP03_PDFHESLO = "#";
            IMP03_CERTFILE = "#";
            IMP03_VZDELISPV = "#";
            IMP03_VZDEL_QRY = "#";
            IMP03_VZDEL_WWW = "#";
        }
        public override string Export()
        {
            StringBuilder b = new StringBuilder("@3||");
            b.Append(ExportString(IMP_OSC));
            b.Append(ExportString(IMP03_VZDELNEJ));
            b.Append(ExportString(IMP03_VZDELOBOR));
            b.Append(ExportString(IMP03_JKOV));
            b.Append(ExportString(IMP03_RIDIC));
            b.Append(ExportString(IMP03_TEL1CIS));
            b.Append(ExportString(IMP03_TEL1TYP));
            b.Append(ExportString(IMP03_TEL2CIS));
            b.Append(ExportString(IMP03_TEL2TYP));
            b.Append(ExportString(IMP03_VOJAK));
            b.Append(ExportString(IMP03_OSDATA));
            b.Append(ExportString(IMP03_PREDCHOZI));
            b.Append(ExportString(IMP03_POZNAMKA));
            b.Append(ExportString(IMP03_PRACMAIL));
            b.Append(ExportString(IMP03_PDFHESLO));
            b.Append(ExportString(IMP03_CERTFILE));
            b.Append(ExportString(IMP03_VZDELISPV));
            b.Append(ExportString(IMP03_VZDEL_QRY));
            b.Append(ExportString(IMP03_VZDEL_WWW));

            return b.ToString();
        }
        public override void ExportToExportFile(StreamWriter writer)
        {
            if (string.IsNullOrEmpty(IMP_OSC))
            {
                return;
            }
            writer.Write("@3||");
            ExportField(writer, IMP_OSC);
            ExportField(writer, IMP03_VZDELNEJ);
            ExportField(writer, IMP03_VZDELOBOR);
            ExportField(writer, IMP03_JKOV);
            ExportField(writer, IMP03_RIDIC);
            ExportField(writer, IMP03_TEL1CIS);
            ExportField(writer, IMP03_TEL1TYP);
            ExportField(writer, IMP03_TEL2CIS);
            ExportField(writer, IMP03_TEL2TYP);
            ExportField(writer, IMP03_VOJAK);
            ExportField(writer, IMP03_OSDATA);
            ExportField(writer, IMP03_PREDCHOZI);
            ExportField(writer, IMP03_POZNAMKA);
            ExportField(writer, IMP03_PRACMAIL);
            ExportField(writer, IMP03_PDFHESLO);
            ExportField(writer, IMP03_CERTFILE);
            ExportField(writer, IMP03_VZDELISPV);
            ExportField(writer, IMP03_VZDEL_QRY);
            ExportField(writer, IMP03_VZDEL_WWW);

            writer.WriteLine();

        }
    }
    public class ImportData17 : ImportData
    {
        public string IMP_OSC;
        public string IMP_POM;
        public string IMP17_DATUMZAC;
        public string IMP17_DATUMKON;
        public string IMP17_CINNOSTSPOJ;
        public string IMP17_PLATCEDANPR;
        public string IMP17_PLATCESPOJ;
        public string IMP17_PLATCEZPOJ;
        public string IMP17_CINNENIEVID;
        public string IMP17_CINBEZDOVOL;
        public string IMP17_CINSTSPRAVA;
        public string IMP17_NAROKTARIF;
        public string IMP17_KZAM;
        public string IMP17_ISCO;
        public string IMP17_PRAXEROKY;
        public string IMP17_PRAXEDNY;
        public string IMP17_TARIFTRIDA;
        public string IMP17_TARIFSTUPEN;
        public string IMP17_PSTUPENAUTO;
        public string IMP17_DRUHPRASTAV;
        public string IMP17_DRUHNASTUPU;
        public string IMP17_SJEDDOBA;
        public string IMP17_DRUHUKONCEN;
        public string IMP17_KVALIFIKACE;
        public string IMP17_FUNKCE;
        public string IMP17_NARDOVOL;
        public string IMP17_NARDODAT;
        public string IMP17_DOVJINA;
        public string IMP17_DOVLETOS;
        public string IMP17_DOVLONI;
        public string IMP17_DOCERPANO;
        public string IMP17_DOPROPLAC;
        public string IMP17_MIN_ZP;
        public string IMP17_DATVYPOV;
        public Int32 IMP17_PRIORITC;


        public ImportData17()
        {
            IMP_OSC = "";
            IMP_POM = "";
            IMP17_DATUMZAC = "#";
            IMP17_DATUMKON = "#";
            IMP17_CINNOSTSPOJ = "#";
            IMP17_PLATCEDANPR = "#";
            IMP17_PLATCESPOJ = "#";
            IMP17_PLATCEZPOJ = "#";
            IMP17_CINNENIEVID = "#";
            IMP17_CINBEZDOVOL = "#";
            IMP17_CINSTSPRAVA = "#";
            IMP17_NAROKTARIF = "#";
            IMP17_KZAM = "#";
            IMP17_ISCO = "#";
            IMP17_PRAXEROKY = "#";
            IMP17_PRAXEDNY = "#";
            IMP17_TARIFTRIDA = "#";
            IMP17_TARIFSTUPEN = "#";
            IMP17_PSTUPENAUTO = "#";
            IMP17_DRUHPRASTAV = "#";
            IMP17_DRUHNASTUPU = "#";
            IMP17_SJEDDOBA = "#";
            IMP17_DRUHUKONCEN = "#";
            IMP17_KVALIFIKACE = "#";
            IMP17_FUNKCE = "#";
            IMP17_NARDOVOL = "#";
            IMP17_NARDODAT = "#";
            IMP17_DOVJINA = "#";
            IMP17_DOVLETOS = "#";
            IMP17_DOVLONI = "#";
            IMP17_DOCERPANO = "#";
            IMP17_DOPROPLAC = "#";
            IMP17_MIN_ZP = "#";
            IMP17_DATVYPOV = "#";
            IMP17_PRIORITC = 0;
        }
        public override string Export()
        {
            StringBuilder b = new StringBuilder("@17||");
            b.Append(ExportString(IMP_OSC));
            b.Append(ExportString(IMP_POM));
            b.Append(ExportString(IMP17_DATUMZAC));
            b.Append(ExportString(IMP17_DATUMKON));
            b.Append(ExportString(IMP17_CINNOSTSPOJ));
            b.Append(ExportString(IMP17_PLATCEDANPR));
            b.Append(ExportString(IMP17_PLATCESPOJ));
            b.Append(ExportString(IMP17_PLATCEZPOJ));
            b.Append(ExportString(IMP17_CINNENIEVID));
            b.Append(ExportString(IMP17_CINBEZDOVOL));
            b.Append(ExportString(IMP17_CINSTSPRAVA));
            b.Append(ExportString(IMP17_NAROKTARIF));
            b.Append(ExportString(IMP17_KZAM));
            b.Append(ExportString(IMP17_ISCO));
            b.Append(ExportString(IMP17_PRAXEROKY));
            b.Append(ExportString(IMP17_PRAXEDNY));
            b.Append(ExportString(IMP17_TARIFTRIDA));
            b.Append(ExportString(IMP17_TARIFSTUPEN));
            b.Append(ExportString(IMP17_PSTUPENAUTO));
            b.Append(ExportString(IMP17_DRUHPRASTAV));
            b.Append(ExportString(IMP17_DRUHNASTUPU));
            b.Append(ExportString(IMP17_SJEDDOBA));
            b.Append(ExportString(IMP17_DRUHUKONCEN));
            b.Append(ExportString(IMP17_KVALIFIKACE));
            b.Append(ExportString(IMP17_FUNKCE));
            b.Append(ExportString(IMP17_NARDOVOL));
            b.Append(ExportString(IMP17_NARDODAT));
            b.Append(ExportString(IMP17_DOVJINA));
            b.Append(ExportString(IMP17_DOVLETOS));
            b.Append(ExportString(IMP17_DOVLONI));
            b.Append(ExportString(IMP17_DOCERPANO));
            b.Append(ExportString(IMP17_DOPROPLAC));
            b.Append(ExportString(IMP17_MIN_ZP));
            b.Append(ExportString(IMP17_DATVYPOV));
            b.Append(ExportString(PriorityCislo()));

            return b.ToString();
        }
        public override void ExportToExportFile(StreamWriter writer)
        {
            if (string.IsNullOrEmpty(IMP_OSC))
            {
                return;
            }
            writer.Write("@17||");
            ExportField(writer, IMP_OSC);
            ExportField(writer, IMP_POM);
            ExportField(writer, IMP17_DATUMZAC);
            ExportField(writer, IMP17_DATUMKON);
            ExportField(writer, CINNOSTSPOJ());
            ExportField(writer, IMP17_PLATCEDANPR);
            ExportField(writer, IMP17_PLATCESPOJ);
            ExportField(writer, IMP17_PLATCEZPOJ);
            ExportField(writer, IMP17_CINNENIEVID);
            ExportField(writer, IMP17_CINBEZDOVOL);
            ExportField(writer, IMP17_CINSTSPRAVA);
            ExportField(writer, IMP17_NAROKTARIF);
            ExportField(writer, IMP17_KZAM);
            ExportField(writer, IMP17_ISCO);
            ExportField(writer, IMP17_PRAXEROKY);
            ExportField(writer, IMP17_PRAXEDNY);
            ExportField(writer, IMP17_TARIFTRIDA);
            ExportField(writer, IMP17_TARIFSTUPEN);
            ExportField(writer, IMP17_PSTUPENAUTO);
            ExportField(writer, IMP17_DRUHPRASTAV);
            ExportField(writer, IMP17_DRUHNASTUPU);
            ExportField(writer, IMP17_SJEDDOBA);
            ExportField(writer, IMP17_DRUHUKONCEN);
            ExportField(writer, IMP17_KVALIFIKACE);
            ExportField(writer, IMP17_FUNKCE);
            ExportField(writer, IMP17_NARDOVOL);
            ExportField(writer, IMP17_NARDODAT);
            ExportField(writer, IMP17_DOVJINA);
            ExportField(writer, IMP17_DOVLETOS);
            ExportField(writer, IMP17_DOVLONI);
            ExportField(writer, IMP17_DOCERPANO);
            ExportField(writer, IMP17_DOPROPLAC);
            ExportField(writer, IMP17_MIN_ZP);
            ExportField(writer, IMP17_DATVYPOV);
            ExportField(writer, PriorityCislo());

            writer.WriteLine();

        }
        public string PriorityCislo()
        {
            string strReturn = "#";
            if (IMP17_PRIORITC != 0)
            {
                strReturn = $"{IMP17_PRIORITC}";
            }
            return strReturn;
        }
        public string CINNOSTSPOJ()
        {
            string strReturn = "1";
            switch (IMP17_CINNOSTSPOJ)
            {
                case "Hlavní PP":
                    strReturn = "1";
                    break;
                case "Dohoda o pracovní činnosti":
                    strReturn = "10";
                    break;
                case "Dohoda o provedení práce":
                    strReturn = "30";
                    break;
                case "Smlouva statutárního orgánu":
                    strReturn = "26"; //28 = S
                    break;
            }
            return strReturn;
        }
    }
    public class ImportData18 : ImportData
    {
        public string IMP_OSC;
        public string IMP_POM;
        public string IMP18_ZPODMEN;
        public string IMP18_DRUHUVAZ;
        public string IMP18_PLNUVAZ;
        public string IMP18_SKUTUVAZ;
        public string IMP18_KALENUVAZ;
        public string IMP18_STR;
        public string IMP18_CIN;
        public string IMP18_ZAK;
        public string IMP18_TURNDELKA;
        public string IMP18_TURNZAC;
        public string IMP18_TURNSMEN; //14
        public string IMP18_TURNSMENZAM; //42
        public string IMP18_TURNKALEN; // 43
        public string IMP18_PRUMVYDHOD; //74
        public string IMP18_ZAPVYDDEN;
        public string IMP18_ZVYSZAKLQ1;
        public string IMP18_ZVYSZAKLQ2;
        public string IMP18_ZVYSZAKLQ3;
        public string IMP18_ZVYSZAKLQ4;
        public string IMP18_DOVVSVATEK;
        public string IMP18_DOVZREZIMU; //81

        public ImportData18()
        {
            IMP_OSC = "";
            IMP_POM = "";
            IMP18_ZPODMEN = "#";
            IMP18_DRUHUVAZ = "#";
            IMP18_PLNUVAZ = "#";
            IMP18_SKUTUVAZ = "#";
            IMP18_KALENUVAZ = "#";
            IMP18_STR = "#";
            IMP18_CIN = "#";
            IMP18_ZAK = "#";
            IMP18_TURNDELKA = "#";
            IMP18_TURNZAC = "#";
            IMP18_TURNSMEN = "#";
            IMP18_TURNSMENZAM = "#";
            IMP18_TURNKALEN = "#";
            IMP18_PRUMVYDHOD = "#";
            IMP18_ZAPVYDDEN = "#";
            IMP18_ZVYSZAKLQ1 = "#";
            IMP18_ZVYSZAKLQ2 = "#";
            IMP18_ZVYSZAKLQ3 = "#";
            IMP18_ZVYSZAKLQ4 = "#";
            IMP18_DOVVSVATEK = "#";
            IMP18_DOVZREZIMU = "#";
        }
        public override string Export()
        {
            StringBuilder b = new StringBuilder("@18||");
            b.Append(ExportString(IMP_OSC));
            b.Append(ExportString(IMP_POM));
            b.Append(ExportString(IMP18_ZPODMEN));
            b.Append(ExportString(IMP18_DRUHUVAZ));
            b.Append(ExportString(IMP18_PLNUVAZ));
            b.Append(ExportString(IMP18_SKUTUVAZ));
            b.Append(ExportString(IMP18_KALENUVAZ));
            b.Append(ExportString(IMP18_STR));
            b.Append(ExportString(IMP18_CIN));
            b.Append(ExportString(IMP18_ZAK));
            b.Append(ExportString(IMP18_TURNDELKA));
            b.Append(ExportString(IMP18_TURNZAC));
            b.Append(ExportString(IMP18_TURNSMEN)); // 1
            b.Append(ExportString(IMP18_TURNSMEN)); // 2
            b.Append(ExportString(IMP18_TURNSMEN)); // 3
            b.Append(ExportString(IMP18_TURNSMEN)); // 4
            b.Append(ExportString(IMP18_TURNSMEN)); // 5
            b.Append(ExportString(IMP18_TURNSMEN)); // 6
            b.Append(ExportString(IMP18_TURNSMEN)); // 7
            b.Append(ExportString(IMP18_TURNSMEN)); // 8
            b.Append(ExportString(IMP18_TURNSMEN)); // 9
            b.Append(ExportString(IMP18_TURNSMEN)); //10 
            b.Append(ExportString(IMP18_TURNSMEN)); // 1
            b.Append(ExportString(IMP18_TURNSMEN)); // 2
            b.Append(ExportString(IMP18_TURNSMEN)); // 3
            b.Append(ExportString(IMP18_TURNSMEN)); // 4
            b.Append(ExportString(IMP18_TURNSMEN)); // 5
            b.Append(ExportString(IMP18_TURNSMEN)); // 6
            b.Append(ExportString(IMP18_TURNSMEN)); // 7
            b.Append(ExportString(IMP18_TURNSMEN)); // 8
            b.Append(ExportString(IMP18_TURNSMEN)); // 9
            b.Append(ExportString(IMP18_TURNSMEN)); //10
            b.Append(ExportString(IMP18_TURNSMEN)); // 1
            b.Append(ExportString(IMP18_TURNSMEN)); // 2
            b.Append(ExportString(IMP18_TURNSMEN)); // 3
            b.Append(ExportString(IMP18_TURNSMEN)); // 4
            b.Append(ExportString(IMP18_TURNSMEN)); // 5
            b.Append(ExportString(IMP18_TURNSMEN)); // 6
            b.Append(ExportString(IMP18_TURNSMEN)); // 7
            b.Append(ExportString(IMP18_TURNSMEN)); // 8
            b.Append(ExportString(IMP18_TURNSMENZAM));
            b.Append(ExportString(IMP18_TURNKALEN));  // 1                
            b.Append(ExportString(IMP18_TURNKALEN)); // 2
            b.Append(ExportString(IMP18_TURNKALEN)); // 3
            b.Append(ExportString(IMP18_TURNKALEN)); // 4
            b.Append(ExportString(IMP18_TURNKALEN)); // 5
            b.Append(ExportString(IMP18_TURNKALEN)); // 6
            b.Append(ExportString(IMP18_TURNKALEN)); // 7
            b.Append(ExportString(IMP18_TURNKALEN)); // 8
            b.Append(ExportString(IMP18_TURNKALEN)); // 9
            b.Append(ExportString(IMP18_TURNKALEN)); //10 
            b.Append(ExportString(IMP18_TURNKALEN)); // 1
            b.Append(ExportString(IMP18_TURNKALEN)); // 2
            b.Append(ExportString(IMP18_TURNKALEN)); // 3
            b.Append(ExportString(IMP18_TURNKALEN)); // 4
            b.Append(ExportString(IMP18_TURNKALEN)); // 5
            b.Append(ExportString(IMP18_TURNKALEN)); // 6
            b.Append(ExportString(IMP18_TURNKALEN)); // 7
            b.Append(ExportString(IMP18_TURNKALEN)); // 8
            b.Append(ExportString(IMP18_TURNKALEN)); // 9
            b.Append(ExportString(IMP18_TURNKALEN)); //10
            b.Append(ExportString(IMP18_TURNKALEN)); // 1
            b.Append(ExportString(IMP18_TURNKALEN)); // 2
            b.Append(ExportString(IMP18_TURNKALEN)); // 3
            b.Append(ExportString(IMP18_TURNKALEN)); // 4
            b.Append(ExportString(IMP18_TURNKALEN)); // 5
            b.Append(ExportString(IMP18_TURNKALEN)); // 6
            b.Append(ExportString(IMP18_TURNKALEN)); // 7
            b.Append(ExportString(IMP18_TURNKALEN)); // 8
            b.Append(ExportString(IMP18_TURNKALEN)); // 9
            b.Append(ExportString(IMP18_TURNKALEN)); //10
            b.Append(ExportString(IMP18_TURNKALEN)); // 1
            b.Append(ExportString(IMP18_PRUMVYDHOD));
            b.Append(ExportString(IMP18_ZAPVYDDEN));
            b.Append(ExportString(IMP18_ZVYSZAKLQ1));
            b.Append(ExportString(IMP18_ZVYSZAKLQ2));
            b.Append(ExportString(IMP18_ZVYSZAKLQ3));
            b.Append(ExportString(IMP18_ZVYSZAKLQ4));
            b.Append(ExportString(IMP18_DOVVSVATEK));
            b.Append(ExportString(IMP18_DOVZREZIMU));

            return b.ToString();
        }
        public override void ExportToExportFile(StreamWriter writer)
        {
            if (string.IsNullOrEmpty(IMP_OSC))
            {
                return;
            }
            writer.Write("@18||");
            ExportField(writer, IMP_OSC);
            ExportField(writer, IMP_POM);
            ExportField(writer, IMP18_ZPODMEN);
            ExportField(writer, IMP18_DRUHUVAZ);
            ExportField(writer, IMP18_PLNUVAZ);
            ExportField(writer, IMP18_SKUTUVAZ);
            ExportField(writer, IMP18_KALENUVAZ);
            ExportField(writer, IMP18_STR);
            ExportField(writer, IMP18_CIN);
            ExportField(writer, IMP18_ZAK);
            ExportField(writer, IMP18_TURNDELKA);
            ExportField(writer, IMP18_TURNZAC);
            ExportField(writer, IMP18_TURNSMEN); // 1
            ExportField(writer, IMP18_TURNSMEN); // 2
            ExportField(writer, IMP18_TURNSMEN); // 3
            ExportField(writer, IMP18_TURNSMEN); // 4
            ExportField(writer, IMP18_TURNSMEN); // 5
            ExportField(writer, IMP18_TURNSMEN); // 6
            ExportField(writer, IMP18_TURNSMEN); // 7
            ExportField(writer, IMP18_TURNSMEN); // 8
            ExportField(writer, IMP18_TURNSMEN); // 9
            ExportField(writer, IMP18_TURNSMEN); //10 
            ExportField(writer, IMP18_TURNSMEN); // 1
            ExportField(writer, IMP18_TURNSMEN); // 2
            ExportField(writer, IMP18_TURNSMEN); // 3
            ExportField(writer, IMP18_TURNSMEN); // 4
            ExportField(writer, IMP18_TURNSMEN); // 5
            ExportField(writer, IMP18_TURNSMEN); // 6
            ExportField(writer, IMP18_TURNSMEN); // 7
            ExportField(writer, IMP18_TURNSMEN); // 8
            ExportField(writer, IMP18_TURNSMEN); // 9
            ExportField(writer, IMP18_TURNSMEN); //10
            ExportField(writer, IMP18_TURNSMEN); // 1
            ExportField(writer, IMP18_TURNSMEN); // 2
            ExportField(writer, IMP18_TURNSMEN); // 3
            ExportField(writer, IMP18_TURNSMEN); // 4
            ExportField(writer, IMP18_TURNSMEN); // 5
            ExportField(writer, IMP18_TURNSMEN); // 6
            ExportField(writer, IMP18_TURNSMEN); // 7
            ExportField(writer, IMP18_TURNSMEN); // 8
            ExportField(writer, IMP18_TURNSMENZAM);
            ExportField(writer, IMP18_TURNKALEN);  // 1                
            ExportField(writer, IMP18_TURNKALEN); // 2
            ExportField(writer, IMP18_TURNKALEN); // 3
            ExportField(writer, IMP18_TURNKALEN); // 4
            ExportField(writer, IMP18_TURNKALEN); // 5
            ExportField(writer, IMP18_TURNKALEN); // 6
            ExportField(writer, IMP18_TURNKALEN); // 7
            ExportField(writer, IMP18_TURNKALEN); // 8
            ExportField(writer, IMP18_TURNKALEN); // 9
            ExportField(writer, IMP18_TURNKALEN); //10 
            ExportField(writer, IMP18_TURNKALEN); // 1
            ExportField(writer, IMP18_TURNKALEN); // 2
            ExportField(writer, IMP18_TURNKALEN); // 3
            ExportField(writer, IMP18_TURNKALEN); // 4
            ExportField(writer, IMP18_TURNKALEN); // 5
            ExportField(writer, IMP18_TURNKALEN); // 6
            ExportField(writer, IMP18_TURNKALEN); // 7
            ExportField(writer, IMP18_TURNKALEN); // 8
            ExportField(writer, IMP18_TURNKALEN); // 9
            ExportField(writer, IMP18_TURNKALEN); //10
            ExportField(writer, IMP18_TURNKALEN); // 1
            ExportField(writer, IMP18_TURNKALEN); // 2
            ExportField(writer, IMP18_TURNKALEN); // 3
            ExportField(writer, IMP18_TURNKALEN); // 4
            ExportField(writer, IMP18_TURNKALEN); // 5
            ExportField(writer, IMP18_TURNKALEN); // 6
            ExportField(writer, IMP18_TURNKALEN); // 7
            ExportField(writer, IMP18_TURNKALEN); // 8
            ExportField(writer, IMP18_TURNKALEN); // 9
            ExportField(writer, IMP18_TURNKALEN); //10
            ExportField(writer, IMP18_TURNKALEN); // 1
            ExportField(writer, IMP18_PRUMVYDHOD);
            ExportField(writer, IMP18_ZAPVYDDEN);
            ExportField(writer, IMP18_ZVYSZAKLQ1);
            ExportField(writer, IMP18_ZVYSZAKLQ2);
            ExportField(writer, IMP18_ZVYSZAKLQ3);
            ExportField(writer, IMP18_ZVYSZAKLQ4);
            ExportField(writer, IMP18_DOVVSVATEK);
            ExportField(writer, IMP18_DOVZREZIMU);

            writer.WriteLine();

        }
    }
    public class ImportData37 : ImportData
    {
        public string IMP_OSC;
        public string IMP_POM;
        public string IMP37_DATUMEVI;
        public string IMP37_DATUMORG;
        public string IMP37_DATUM_NP;
        public string IMP37_DATUM_ZP;

        public ImportData37()
        {
            IMP_OSC = "";
            IMP_POM = "";
            IMP37_DATUMEVI = "#";
            IMP37_DATUMORG = "#";
            IMP37_DATUM_NP = "#";
            IMP37_DATUM_ZP = "#";
        }
        public override string Export()
        {
            StringBuilder b = new StringBuilder("@37||");
            b.Append(ExportString(IMP_OSC));
            b.Append(ExportString(IMP_POM));
            b.Append(ExportString(IMP37_DATUMEVI));
            b.Append(ExportString(IMP37_DATUMORG));
            b.Append(ExportString(IMP37_DATUM_NP));
            b.Append(ExportString(IMP37_DATUM_ZP));

            return b.ToString();
        }
        public override void ExportToExportFile(StreamWriter writer)
        {
            if (string.IsNullOrEmpty(IMP_OSC))
            {
                return;
            }
            writer.Write("@37||");
            ExportField(writer, IMP_OSC);
            ExportField(writer, IMP_POM);
            ExportField(writer, IMP37_DATUMEVI);
            ExportField(writer, IMP37_DATUMORG);
            ExportField(writer, IMP37_DATUM_NP);
            ExportField(writer, IMP37_DATUM_ZP);

            writer.WriteLine();

        }
    }
    public class ImportData50 : ImportData
    {
        public string IMP_OSC;
        public string IMP_POM;
        public string IMP50_DOV_NROK;
        public string IMP50_DOB_RZAD;
        public string IMP50_DOV_CERP;
        public string IMP50_DOV_PROP;
        public string IMP50_DOV_VRAC;
        public string IMP50_DOV_KRAC;
        public string IMP50_LON_CERP;
        public string IMP50_LON_PROP;
        public string IMP50_LON_VRAC;
        public string IMP50_LON_KRAC;


        public ImportData50()
        {
            IMP_OSC = "";
            IMP_POM = "";
            IMP50_DOV_NROK = "#";
            IMP50_DOB_RZAD = "#";
            IMP50_DOV_CERP = "#";
            IMP50_DOV_PROP = "#";
            IMP50_DOV_VRAC = "#";
            IMP50_DOV_KRAC = "#";
            IMP50_LON_CERP = "#";
            IMP50_LON_PROP = "#";
            IMP50_LON_VRAC = "#";
            IMP50_LON_KRAC = "#";
        }
        public override string Export()
        {
            StringBuilder b = new StringBuilder("@50||");
            b.Append(ExportString(IMP_OSC));
            b.Append(ExportString(IMP_POM));
            b.Append(ExportString(IMP50_DOV_NROK));
            b.Append(ExportString(IMP50_DOB_RZAD));
            b.Append(ExportString(IMP50_DOV_CERP));
            b.Append(ExportString(IMP50_DOV_PROP));
            b.Append(ExportString(IMP50_DOV_VRAC));
            b.Append(ExportString(IMP50_DOV_KRAC));
            b.Append(ExportString(IMP50_LON_CERP));
            b.Append(ExportString(IMP50_LON_PROP));
            b.Append(ExportString(IMP50_LON_VRAC));
            b.Append(ExportString(IMP50_LON_KRAC));

            return b.ToString();
        }
        public override void ExportToExportFile(StreamWriter writer)
        {
            if (string.IsNullOrEmpty(IMP_OSC))
            {
                return;
            }
            writer.Write("@50||");
            ExportField(writer, IMP_OSC);
            ExportField(writer, IMP_POM);
            ExportField(writer, IMP50_DOV_NROK);
            ExportField(writer, IMP50_DOB_RZAD);
            ExportField(writer, IMP50_DOV_CERP);
            ExportField(writer, IMP50_DOV_PROP);
            ExportField(writer, IMP50_DOV_VRAC);
            ExportField(writer, IMP50_DOV_KRAC);
            ExportField(writer, IMP50_LON_CERP);
            ExportField(writer, IMP50_LON_PROP);
            ExportField(writer, IMP50_LON_VRAC);
            ExportField(writer, IMP50_LON_KRAC);

            writer.WriteLine();

        }
    }
    public class ImportData58 : ImportData
    {
        public string IMP_OSC;
        public string IMP_POM;
        public string IMP58_PRUMVYDHOD;
        public string IMP58_ZAPVYDDEN;
        public string IMP58_ZVYSZAKLQ1;
        public string IMP58_ZVYSZAKLQ2;
        public string IMP58_ZVYSZAKLQ3;
        public string IMP58_ZVYSZAKLQ4;

        public ImportData58()
        {
            IMP_OSC = "";
            IMP_POM = "";
            IMP58_PRUMVYDHOD = "#";
            IMP58_ZAPVYDDEN = "#";
            IMP58_ZVYSZAKLQ1 = "#";
            IMP58_ZVYSZAKLQ2 = "#";
            IMP58_ZVYSZAKLQ3 = "#";
            IMP58_ZVYSZAKLQ4 = "#";
        }
        public override string Export()
        {
            StringBuilder b = new StringBuilder("@58||");
            b.Append(ExportString(IMP_OSC));
            b.Append(ExportString(IMP_POM));
            b.Append(ExportString(IMP58_PRUMVYDHOD));
            b.Append(ExportString(IMP58_ZAPVYDDEN));
            b.Append(ExportString(IMP58_ZVYSZAKLQ1));
            b.Append(ExportString(IMP58_ZVYSZAKLQ2));
            b.Append(ExportString(IMP58_ZVYSZAKLQ3));
            b.Append(ExportString(IMP58_ZVYSZAKLQ4));

            return b.ToString();
        }
        public override void ExportToExportFile(StreamWriter writer)
        {
            if (string.IsNullOrEmpty(IMP_OSC))
            {
                return;
            }
            writer.Write("@58||");
            ExportField(writer, IMP_OSC);
            ExportField(writer, IMP_POM);
            ExportField(writer, IMP58_PRUMVYDHOD);
            ExportField(writer, IMP58_ZAPVYDDEN);
            ExportField(writer, IMP58_ZVYSZAKLQ1);
            ExportField(writer, IMP58_ZVYSZAKLQ2);
            ExportField(writer, IMP58_ZVYSZAKLQ3);
            ExportField(writer, IMP58_ZVYSZAKLQ4);

            writer.WriteLine();

        }
    }
    public class ImportData26 : ImportData
    {
        public string IMP_OSC;
        public string IMP_POM;
        public string IMP26_OBDOBIMES;
        public string IMP26_OBDOBIROK;
        public string IMP26_ZVYSENI1Q;
        public string IMP26_ZVYSENI2Q;
        public string IMP26_ZVYSENI3Q;
        public string IMP26_ZVYSENI4Q;

        public ImportData26()
        {
            IMP_OSC = "";
            IMP_POM = "";
            IMP26_OBDOBIMES = "#";
            IMP26_OBDOBIROK = "#";
            IMP26_ZVYSENI1Q = "#";
            IMP26_ZVYSENI2Q = "#";
            IMP26_ZVYSENI3Q = "#";
            IMP26_ZVYSENI4Q = "#";
        }
        public override string Export()
        {
            StringBuilder b = new StringBuilder("@26||");
            b.Append(ExportString(IMP_OSC));
            b.Append(ExportString(IMP_POM));
            b.Append(ExportString(IMP26_OBDOBIMES));
            b.Append(ExportString(IMP26_OBDOBIROK));
            b.Append(ExportString(IMP26_ZVYSENI1Q));
            b.Append(ExportString(IMP26_ZVYSENI2Q));
            b.Append(ExportString(IMP26_ZVYSENI3Q));
            b.Append(ExportString(IMP26_ZVYSENI4Q));

            return b.ToString();
        }
        public override void ExportToExportFile(StreamWriter writer)
        {
            if (string.IsNullOrEmpty(IMP_OSC))
            {
                return;
            }
            writer.Write("@26||");
            ExportField(writer, IMP_OSC);
            ExportField(writer, IMP_POM);
            ExportField(writer, IMP26_OBDOBIMES);
            ExportField(writer, IMP26_OBDOBIROK);
            ExportField(writer, IMP26_ZVYSENI1Q);
            ExportField(writer, IMP26_ZVYSENI2Q);
            ExportField(writer, IMP26_ZVYSENI3Q);
            ExportField(writer, IMP26_ZVYSENI4Q);

            writer.WriteLine();

        }
    }
    public class ImportData67 : ImportData
    {
        public string IMP_OSC;
        public string IMP_ROK;
        public string IMP_MES;
        public string IMP67_NEZ_PRIJEM;
        public string IMP67_NEZ_POJISTNE;
        public string IMP67_ZAL_PRIJEM;
        public string IMP67_ZAL_POJISTNE;
        public string IMP67_SRZ_PRIJEM;
        public string IMP67_SRZ_POJISTNE;
        public string IMP67_ZAH_POJISTNE;
        public string IMP67_ZALOHA;
        public string IMP67_SRAZKA;
        public string IMP67_SLEVAB;
        public string IMP67_SLEVAC;
        public string IMP67_BONUSC;
        public string IMP67_PENZ_PRISP;
        public string IMP67_PENZ_OSVOB;
        public string IMP67_ZIVP_PRISP;
        public string IMP67_ZIVP_OSVOB;
        public string IMP67_PROH_PODEP;
        public string IMP67_PROH_SPOPL;
        public string IMP67_PROH_SINV1;
        public string IMP67_PROH_SINV2;
        public string IMP67_PROH_SINV3;
        public string IMP67_OPERATION;

        public ImportData67()
        {
            IMP_OSC = "";
            IMP_ROK = "";
            IMP_MES = "";
            IMP67_NEZ_PRIJEM = "#";
            IMP67_NEZ_POJISTNE = "#";
            IMP67_ZAL_PRIJEM = "#";
            IMP67_ZAL_POJISTNE = "#";
            IMP67_SRZ_PRIJEM = "#";
            IMP67_SRZ_POJISTNE = "#";
            IMP67_ZAH_POJISTNE = "#";
            IMP67_ZALOHA = "#";
            IMP67_SRAZKA = "#";
            IMP67_SLEVAB = "#";
            IMP67_SLEVAC = "#";
            IMP67_BONUSC = "#";
            IMP67_PENZ_PRISP = "#";
            IMP67_PENZ_OSVOB = "#";
            IMP67_ZIVP_PRISP = "#";
            IMP67_ZIVP_OSVOB = "#";
            IMP67_PROH_PODEP = "#";
            IMP67_PROH_SPOPL = "#";
            IMP67_PROH_SINV1 = "#";
            IMP67_PROH_SINV2 = "#";
            IMP67_PROH_SINV3 = "#";
            IMP67_OPERATION = "#";
        }
        public override string Export()
        {
            StringBuilder b = new StringBuilder("@67||");
            b.Append(ExportString(IMP_OSC));
            b.Append(ExportString(IMP_ROK));
            b.Append(ExportString(IMP_MES));
            b.Append(ExportString(IMP67_NEZ_PRIJEM));
            b.Append(ExportString(IMP67_NEZ_POJISTNE));
            b.Append(ExportString(IMP67_ZAL_PRIJEM));
            b.Append(ExportString(IMP67_ZAL_POJISTNE));
            b.Append(ExportString(IMP67_SRZ_PRIJEM));
            b.Append(ExportString(IMP67_SRZ_POJISTNE));
            b.Append(ExportString(IMP67_ZAH_POJISTNE));
            b.Append(ExportString(IMP67_ZALOHA));
            b.Append(ExportString(IMP67_SRAZKA));
            b.Append(ExportString(IMP67_SLEVAB));
            b.Append(ExportString(IMP67_SLEVAC));
            b.Append(ExportString(IMP67_BONUSC));
            b.Append(ExportString(IMP67_PENZ_PRISP));
            b.Append(ExportString(IMP67_PENZ_OSVOB));
            b.Append(ExportString(IMP67_ZIVP_PRISP));
            b.Append(ExportString(IMP67_ZIVP_OSVOB));
            b.Append(ExportString(IMP67_PROH_PODEP));
            b.Append(ExportString(IMP67_PROH_SPOPL));
            b.Append(ExportString(IMP67_PROH_SINV1));
            b.Append(ExportString(IMP67_PROH_SINV2));
            b.Append(ExportString(IMP67_PROH_SINV3));
            b.Append(ExportString(IMP67_OPERATION));

            return b.ToString();
        }
        public override void ExportToExportFile(StreamWriter writer)
        {
            if (string.IsNullOrEmpty(IMP_OSC))
            {
                return;
            }
            if (string.IsNullOrEmpty(IMP_ROK))
            {
                return;
            }
            if (string.IsNullOrEmpty(IMP_MES))
            {
                return;
            }
            writer.Write("@67||");
            ExportField(writer, IMP_OSC);
            ExportField(writer, IMP_ROK);
            ExportField(writer, IMP_MES);
            ExportField(writer, IMP67_NEZ_PRIJEM);
            ExportField(writer, IMP67_NEZ_POJISTNE);
            ExportField(writer, IMP67_ZAL_PRIJEM);
            ExportField(writer, IMP67_ZAL_POJISTNE);
            ExportField(writer, IMP67_SRZ_PRIJEM);
            ExportField(writer, IMP67_SRZ_POJISTNE);
            ExportField(writer, IMP67_ZAH_POJISTNE);
            ExportField(writer, IMP67_ZALOHA);
            ExportField(writer, IMP67_SRAZKA);
            ExportField(writer, IMP67_SLEVAB);
            ExportField(writer, IMP67_SLEVAC);
            ExportField(writer, IMP67_BONUSC);
            ExportField(writer, IMP67_PENZ_PRISP);
            ExportField(writer, IMP67_PENZ_OSVOB);
            ExportField(writer, IMP67_ZIVP_PRISP);
            ExportField(writer, IMP67_ZIVP_OSVOB);
            ExportField(writer, IMP67_PROH_PODEP);
            ExportField(writer, IMP67_PROH_SPOPL);
            ExportField(writer, IMP67_PROH_SINV1);
            ExportField(writer, IMP67_PROH_SINV2);
            ExportField(writer, IMP67_PROH_SINV3);
            ExportField(writer, IMP67_OPERATION);

            writer.WriteLine();

        }
    }
    public class ImportData77 : ImportData
    {
        public string IMP_OSC;
        public string IMP_ROK;
        public string IMP77_NEZ_PRIJEM;
        public string IMP77_NEZ_POJISTNE;
        public string IMP77_ZAL_PRIJEM;
        public string IMP77_ZAL_POJISTNE;
        public string IMP77_SRZ_PRIJEM;
        public string IMP77_SRZ_POJISTNE;
        public string IMP77_ZAH_POJISTNE;
        public string IMP77_ZALOHA;
        public string IMP77_SRAZKA;
        public string IMP77_SLEVAB;
        public string IMP77_SLEVAC;
        public string IMP77_BONUSC;
        public string IMP77_PENZ_PRPOJ;
        public string IMP77_PENZ_PRISP;
        public string IMP77_PENZ_OSVOB;
        public string IMP77_ZIVP_PRPOJ;
        public string IMP77_ZIVP_PRISP;
        public string IMP77_ZIVP_OSVOB;
        public string IMP77_OPERATION;

        public Int32 ROK_NEZ_PRIJEM;
        public Int32 ROK_NEZ_POJISTNE;
        public Int32 ROK_ZAL_PRIJEM;
        public Int32 ROK_ZAL_POJISTNE;
        public Int32 ROK_SRZ_PRIJEM;
        public Int32 ROK_SRZ_POJISTNE;
        public Int32 ROK_ZAH_POJISTNE;
        public Int32 ROK_ZALOHA;
        public Int32 ROK_SRAZKA;
        public Int32 ROK_SLEVAB;
        public Int32 ROK_SLEVAC;
        public Int32 ROK_BONUSC;
        public Int32 ROK_PENZ_PRPOJ;
        public Int32 ROK_PENZ_PRISP;
        public Int32 ROK_PENZ_OSVOB;
        public Int32 ROK_ZIVP_PRPOJ;
        public Int32 ROK_ZIVP_PRISP;
        public Int32 ROK_ZIVP_OSVOB;

        public ImportData77()
        {
            IMP_OSC = "";
            IMP_ROK = "";
            IMP77_NEZ_PRIJEM = "#";
            IMP77_NEZ_POJISTNE = "#";
            IMP77_ZAL_PRIJEM = "#";
            IMP77_ZAL_POJISTNE = "#";
            IMP77_SRZ_PRIJEM = "#";
            IMP77_SRZ_POJISTNE = "#";
            IMP77_ZAH_POJISTNE = "#";
            IMP77_ZALOHA = "#";
            IMP77_SRAZKA = "#";
            IMP77_SLEVAB = "#";
            IMP77_SLEVAC = "#";
            IMP77_BONUSC = "#";
            IMP77_PENZ_PRPOJ = "#";
            IMP77_PENZ_PRISP = "#";
            IMP77_PENZ_OSVOB = "#";
            IMP77_ZIVP_PRPOJ = "#";
            IMP77_ZIVP_PRISP = "#";
            IMP77_ZIVP_OSVOB = "#";
            IMP77_OPERATION = "0";

            ROK_NEZ_PRIJEM = 0;
            ROK_NEZ_POJISTNE = 0;
            ROK_ZAL_PRIJEM = 0;
            ROK_ZAL_POJISTNE = 0;
            ROK_SRZ_PRIJEM = 0;
            ROK_SRZ_POJISTNE = 0;
            ROK_ZAH_POJISTNE = 0;
            ROK_ZALOHA = 0;
            ROK_SRAZKA = 0;
            ROK_SLEVAB = 0;
            ROK_SLEVAC = 0;
            ROK_BONUSC = 0;
            ROK_PENZ_PRPOJ = 0;
            ROK_PENZ_PRISP = 0;
            ROK_PENZ_OSVOB = 0;
            ROK_ZIVP_PRPOJ = 0;
            ROK_ZIVP_PRISP = 0;
            ROK_ZIVP_OSVOB = 0;
        }
        public override string Export()
        {
            IMP77_ZAL_PRIJEM = EXP_ZAL_PRIJEM();
            IMP77_ZAL_POJISTNE = EXP_ZAL_POJISTNE();
            IMP77_SRZ_PRIJEM = EXP_SRZ_PRIJEM();
            IMP77_SRZ_POJISTNE = EXP_SRZ_POJISTNE();
            IMP77_ZALOHA = EXP_ZALOHA();
            IMP77_SRAZKA = EXP_SRAZKA();
            IMP77_SLEVAB = EXP_SLEVAB();
            StringBuilder b = new StringBuilder("@77||");
            b.Append(ExportString(IMP_OSC));
            b.Append(ExportString(IMP_ROK));
            b.Append(ExportString(IMP77_NEZ_PRIJEM));
            b.Append(ExportString(IMP77_NEZ_POJISTNE));
            b.Append(ExportString(IMP77_ZAL_PRIJEM));
            b.Append(ExportString(IMP77_ZAL_POJISTNE));
            b.Append(ExportString(IMP77_SRZ_PRIJEM));
            b.Append(ExportString(IMP77_SRZ_POJISTNE));
            b.Append(ExportString(IMP77_ZAH_POJISTNE));
            b.Append(ExportString(IMP77_ZALOHA));
            b.Append(ExportString(IMP77_SRAZKA));
            b.Append(ExportString(IMP77_SLEVAB));
            b.Append(ExportString(IMP77_SLEVAC));
            b.Append(ExportString(IMP77_BONUSC));
            b.Append(ExportString(IMP77_PENZ_PRPOJ));
            b.Append(ExportString(IMP77_PENZ_PRISP));
            b.Append(ExportString(IMP77_PENZ_OSVOB));
            b.Append(ExportString(IMP77_ZIVP_PRPOJ));
            b.Append(ExportString(IMP77_ZIVP_PRISP));
            b.Append(ExportString(IMP77_ZIVP_OSVOB));
            b.Append(ExportString(IMP77_OPERATION));

            return b.ToString();
        }
        public override void ExportToExportFile(StreamWriter writer)
        {
            if (string.IsNullOrEmpty(IMP_OSC))
            {
                return;
            }
            if (string.IsNullOrEmpty(IMP_ROK))
            {
                return;
            }
            IMP77_ZAL_PRIJEM = EXP_ZAL_PRIJEM();
            IMP77_ZAL_POJISTNE = EXP_ZAL_POJISTNE();
            IMP77_SRZ_PRIJEM = EXP_SRZ_PRIJEM();
            IMP77_SRZ_POJISTNE = EXP_SRZ_POJISTNE();
            IMP77_ZALOHA = EXP_ZALOHA();
            IMP77_SRAZKA = EXP_SRAZKA();
            IMP77_SLEVAB = EXP_SLEVAB();

            writer.Write("@77||");
            ExportField(writer, IMP_OSC);
            ExportField(writer, IMP_ROK);
            ExportField(writer, IMP77_NEZ_PRIJEM);
            ExportField(writer, IMP77_NEZ_POJISTNE);
            ExportField(writer, IMP77_ZAL_PRIJEM);
            ExportField(writer, IMP77_ZAL_POJISTNE);
            ExportField(writer, IMP77_SRZ_PRIJEM);
            ExportField(writer, IMP77_SRZ_POJISTNE);
            ExportField(writer, IMP77_ZAH_POJISTNE);
            ExportField(writer, IMP77_ZALOHA);
            ExportField(writer, IMP77_SRAZKA);
            ExportField(writer, IMP77_SLEVAB);
            ExportField(writer, IMP77_SLEVAC);
            ExportField(writer, IMP77_BONUSC);
            ExportField(writer, IMP77_PENZ_PRPOJ);
            ExportField(writer, IMP77_PENZ_PRISP);
            ExportField(writer, IMP77_PENZ_OSVOB);
            ExportField(writer, IMP77_ZIVP_PRPOJ);
            ExportField(writer, IMP77_ZIVP_PRISP);
            ExportField(writer, IMP77_ZIVP_OSVOB);
            ExportField(writer, IMP77_OPERATION);

            writer.WriteLine();

        }

        public string EXP_NEZ_PRIJEM() { return ROK_NEZ_PRIJEM.ToString(); }
        public string EXP_NEZ_POJISTNE() { return ROK_NEZ_POJISTNE.ToString(); }
        public string EXP_ZAL_PRIJEM() { return ROK_ZAL_PRIJEM.ToString(); }
        public string EXP_ZAL_POJISTNE() { return ROK_ZAL_POJISTNE.ToString(); }
        public string EXP_SRZ_PRIJEM() { return ROK_SRZ_PRIJEM.ToString(); }
        public string EXP_SRZ_POJISTNE() { return ROK_SRZ_POJISTNE.ToString(); }
        public string EXP_ZAH_POJISTNE() { return ROK_ZAH_POJISTNE.ToString(); }
        public string EXP_ZALOHA() { return ROK_ZALOHA.ToString(); }
        public string EXP_SRAZKA() { return ROK_SRAZKA.ToString(); }
        public string EXP_SLEVAB() { return ROK_SLEVAB.ToString(); }
        public string EXP_SLEVAC() { return ROK_SLEVAC.ToString(); }
        public string EXP_BONUSC() { return ROK_BONUSC.ToString(); }
        public string EXP_PENZ_PRPOJ() { return ROK_PENZ_PRPOJ.ToString(); }
        public string EXP_PENZ_PRISP() { return ROK_PENZ_PRISP.ToString(); }
        public string EXP_PENZ_OSVOB() { return ROK_PENZ_OSVOB.ToString(); }
        public string EXP_ZIVP_PRPOJ() { return ROK_ZIVP_PRPOJ.ToString(); }
        public string EXP_ZIVP_PRISP() { return ROK_ZIVP_PRISP.ToString(); }
        public string EXP_ZIVP_OSVOB() { return ROK_ZIVP_OSVOB.ToString(); }
    }
    public class ImportData68 : ImportData
    {
        public string IMP_OSC;
        public string IMP_POM;
        public string IMP68_PRAC_ZDR;
        public string IMP68_PRAC_STR;
        public string IMP68_PRAC_CIN;
        public string IMP68_PRAC_ZAK;
        public string IMP68_UVAZ_ZDR;
        public string IMP68_UVAZ_STR;
        public string IMP68_UVAZ_CIN;
        public string IMP68_UVAZ_ZAK;
        public string IMP68_ZAR_FUNKCE;
        public string IMP68_ZAR_KZAM;
        public string IMP68_ZAR_ISCO;
        public string IMP68_ZAR_ICSE;
        public string IMP68_ZAR_VEDR;

        public ImportData68()
        {
            IMP_OSC = "";
            IMP_POM = "";
            IMP68_PRAC_ZDR = "#";
            IMP68_PRAC_STR = "#";
            IMP68_PRAC_CIN = "#";
            IMP68_PRAC_ZAK = "#";
            IMP68_UVAZ_ZDR = "#";
            IMP68_UVAZ_STR = "#";
            IMP68_UVAZ_CIN = "#";
            IMP68_UVAZ_ZAK = "#";
            IMP68_ZAR_FUNKCE = "#";
            IMP68_ZAR_KZAM = "#";
            IMP68_ZAR_ISCO = "#";
            IMP68_ZAR_ICSE = "#";
            IMP68_ZAR_VEDR = "#";
        }
        public override string Export()
        {
            StringBuilder b = new StringBuilder("@68||");
            b.Append(ExportString(IMP_OSC));
            b.Append(ExportString(IMP_POM));
            b.Append(ExportString(IMP68_PRAC_ZDR));
            b.Append(ExportString(IMP68_PRAC_STR));
            b.Append(ExportString(IMP68_PRAC_CIN));
            b.Append(ExportString(IMP68_PRAC_ZAK));
            b.Append(ExportString(IMP68_UVAZ_ZDR));
            b.Append(ExportString(IMP68_UVAZ_STR));
            b.Append(ExportString(IMP68_UVAZ_CIN));
            b.Append(ExportString(IMP68_UVAZ_ZAK));
            b.Append(ExportString(IMP68_ZAR_FUNKCE));
            b.Append(ExportString(IMP68_ZAR_KZAM));
            b.Append(ExportString(IMP68_ZAR_ISCO));
            b.Append(ExportString(IMP68_ZAR_ICSE));
            b.Append(ExportString(IMP68_ZAR_VEDR));

            return b.ToString();
        }
        public override void ExportToExportFile(StreamWriter writer)
        {
            if (string.IsNullOrEmpty(IMP_OSC))
            {
                return;
            }
            writer.Write("@68||");
            ExportField(writer, IMP_OSC);
            ExportField(writer, IMP_POM);
            ExportField(writer, IMP68_PRAC_ZDR);
            ExportField(writer, IMP68_PRAC_STR);
            ExportField(writer, IMP68_PRAC_CIN);
            ExportField(writer, IMP68_PRAC_ZAK);
            ExportField(writer, IMP68_UVAZ_ZDR);
            ExportField(writer, IMP68_UVAZ_STR);
            ExportField(writer, IMP68_UVAZ_CIN);
            ExportField(writer, IMP68_UVAZ_ZAK);
            ExportField(writer, IMP68_ZAR_FUNKCE);
            ExportField(writer, IMP68_ZAR_KZAM);
            ExportField(writer, IMP68_ZAR_ISCO);
            ExportField(writer, IMP68_ZAR_ICSE);
            ExportField(writer, IMP68_ZAR_VEDR);

            writer.WriteLine();

        }
    }
    public class ImportData69 : ImportData
    {
        public string IMP_OSC;
        public string IMP_POM;
        public string IMP69_DOV_NROK;
        public string IMP69_DOV_UVAZ;
        public string IMP69_DOV_ZMES;
        public string IMP69_DOB_RZAD;
        public string IMP69_M01_UVAZ;
        public string IMP69_M02_UVAZ;
        public string IMP69_M03_UVAZ;
        public string IMP69_M04_UVAZ;
        public string IMP69_M05_UVAZ;
        public string IMP69_M06_UVAZ;
        public string IMP69_M07_UVAZ;
        public string IMP69_M08_UVAZ;
        public string IMP69_M09_UVAZ;
        public string IMP69_M10_UVAZ;
        public string IMP69_M11_UVAZ;
        public string IMP69_M12_UVAZ;
        public string IMP69_M01_ODPR;
        public string IMP69_M02_ODPR;
        public string IMP69_M03_ODPR;
        public string IMP69_M04_ODPR;
        public string IMP69_M05_ODPR;
        public string IMP69_M06_ODPR;
        public string IMP69_M07_ODPR;
        public string IMP69_M08_ODPR;
        public string IMP69_M09_ODPR;
        public string IMP69_M10_ODPR;
        public string IMP69_M11_ODPR;
        public string IMP69_M12_ODPR;
        public string IMP69_M01_ZAPO;
        public string IMP69_M02_ZAPO;
        public string IMP69_M03_ZAPO;
        public string IMP69_M04_ZAPO;
        public string IMP69_M05_ZAPO;
        public string IMP69_M06_ZAPO;
        public string IMP69_M07_ZAPO;
        public string IMP69_M08_ZAPO;
        public string IMP69_M09_ZAPO;
        public string IMP69_M10_ZAPO;
        public string IMP69_M11_ZAPO;
        public string IMP69_M12_ZAPO;
        public string IMP69_M01_NEPR;
        public string IMP69_M02_NEPR;
        public string IMP69_M03_NEPR;
        public string IMP69_M04_NEPR;
        public string IMP69_M05_NEPR;
        public string IMP69_M06_NEPR;
        public string IMP69_M07_NEPR;
        public string IMP69_M08_NEPR;
        public string IMP69_M09_NEPR;
        public string IMP69_M10_NEPR;
        public string IMP69_M11_NEPR;
        public string IMP69_M12_NEPR;
        public string IMP69_M01_ABSE;
        public string IMP69_M02_ABSE;
        public string IMP69_M03_ABSE;
        public string IMP69_M04_ABSE;
        public string IMP69_M05_ABSE;
        public string IMP69_M06_ABSE;
        public string IMP69_M07_ABSE;
        public string IMP69_M08_ABSE;
        public string IMP69_M09_ABSE;
        public string IMP69_M10_ABSE;
        public string IMP69_M11_ABSE;
        public string IMP69_M12_ABSE;

        public ImportData69()
        {
            IMP_OSC = "";
            IMP_POM = "";
            IMP69_DOV_NROK = "#";
            IMP69_DOV_UVAZ = "#";
            IMP69_DOV_ZMES = "#";
            IMP69_DOB_RZAD = "#";
            IMP69_M01_UVAZ = "#";
            IMP69_M02_UVAZ = "#";
            IMP69_M03_UVAZ = "#";
            IMP69_M04_UVAZ = "#";
            IMP69_M05_UVAZ = "#";
            IMP69_M06_UVAZ = "#";
            IMP69_M07_UVAZ = "#";
            IMP69_M08_UVAZ = "#";
            IMP69_M09_UVAZ = "#";
            IMP69_M10_UVAZ = "#";
            IMP69_M11_UVAZ = "#";
            IMP69_M12_UVAZ = "#";
            IMP69_M01_ODPR = "#";
            IMP69_M02_ODPR = "#";
            IMP69_M03_ODPR = "#";
            IMP69_M04_ODPR = "#";
            IMP69_M05_ODPR = "#";
            IMP69_M06_ODPR = "#";
            IMP69_M07_ODPR = "#";
            IMP69_M08_ODPR = "#";
            IMP69_M09_ODPR = "#";
            IMP69_M10_ODPR = "#";
            IMP69_M11_ODPR = "#";
            IMP69_M12_ODPR = "#";
            IMP69_M01_ZAPO = "#";
            IMP69_M02_ZAPO = "#";
            IMP69_M03_ZAPO = "#";
            IMP69_M04_ZAPO = "#";
            IMP69_M05_ZAPO = "#";
            IMP69_M06_ZAPO = "#";
            IMP69_M07_ZAPO = "#";
            IMP69_M08_ZAPO = "#";
            IMP69_M09_ZAPO = "#";
            IMP69_M10_ZAPO = "#";
            IMP69_M11_ZAPO = "#";
            IMP69_M12_ZAPO = "#";
            IMP69_M01_NEPR = "#";
            IMP69_M02_NEPR = "#";
            IMP69_M03_NEPR = "#";
            IMP69_M04_NEPR = "#";
            IMP69_M05_NEPR = "#";
            IMP69_M06_NEPR = "#";
            IMP69_M07_NEPR = "#";
            IMP69_M08_NEPR = "#";
            IMP69_M09_NEPR = "#";
            IMP69_M10_NEPR = "#";
            IMP69_M11_NEPR = "#";
            IMP69_M12_NEPR = "#";
            IMP69_M01_ABSE = "#";
            IMP69_M02_ABSE = "#";
            IMP69_M03_ABSE = "#";
            IMP69_M04_ABSE = "#";
            IMP69_M05_ABSE = "#";
            IMP69_M06_ABSE = "#";
            IMP69_M07_ABSE = "#";
            IMP69_M08_ABSE = "#";
            IMP69_M09_ABSE = "#";
            IMP69_M10_ABSE = "#";
            IMP69_M11_ABSE = "#";
            IMP69_M12_ABSE = "#";
        }
        public override string Export()
        {
            StringBuilder b = new StringBuilder("@69||");
            b.Append(ExportString(IMP_OSC));
            b.Append(ExportString(IMP_POM));
            b.Append(ExportString(IMP69_DOV_NROK));
            b.Append(ExportString(IMP69_DOV_UVAZ));
            b.Append(ExportString(IMP69_DOV_ZMES));
            b.Append(ExportString(IMP69_DOB_RZAD));
            b.Append(ExportString(IMP69_M01_UVAZ));
            b.Append(ExportString(IMP69_M02_UVAZ));
            b.Append(ExportString(IMP69_M03_UVAZ));
            b.Append(ExportString(IMP69_M04_UVAZ));
            b.Append(ExportString(IMP69_M05_UVAZ));
            b.Append(ExportString(IMP69_M06_UVAZ));
            b.Append(ExportString(IMP69_M07_UVAZ));
            b.Append(ExportString(IMP69_M08_UVAZ));
            b.Append(ExportString(IMP69_M09_UVAZ));
            b.Append(ExportString(IMP69_M10_UVAZ));
            b.Append(ExportString(IMP69_M11_UVAZ));
            b.Append(ExportString(IMP69_M12_UVAZ));
            b.Append(ExportString(IMP69_M01_ODPR));
            b.Append(ExportString(IMP69_M02_ODPR));
            b.Append(ExportString(IMP69_M03_ODPR));
            b.Append(ExportString(IMP69_M04_ODPR));
            b.Append(ExportString(IMP69_M05_ODPR));
            b.Append(ExportString(IMP69_M06_ODPR));
            b.Append(ExportString(IMP69_M07_ODPR));
            b.Append(ExportString(IMP69_M08_ODPR));
            b.Append(ExportString(IMP69_M09_ODPR));
            b.Append(ExportString(IMP69_M10_ODPR));
            b.Append(ExportString(IMP69_M11_ODPR));
            b.Append(ExportString(IMP69_M12_ODPR));
            b.Append(ExportString(IMP69_M01_ZAPO));
            b.Append(ExportString(IMP69_M02_ZAPO));
            b.Append(ExportString(IMP69_M03_ZAPO));
            b.Append(ExportString(IMP69_M04_ZAPO));
            b.Append(ExportString(IMP69_M05_ZAPO));
            b.Append(ExportString(IMP69_M06_ZAPO));
            b.Append(ExportString(IMP69_M07_ZAPO));
            b.Append(ExportString(IMP69_M08_ZAPO));
            b.Append(ExportString(IMP69_M09_ZAPO));
            b.Append(ExportString(IMP69_M10_ZAPO));
            b.Append(ExportString(IMP69_M11_ZAPO));
            b.Append(ExportString(IMP69_M12_ZAPO));
            b.Append(ExportString(IMP69_M01_NEPR));
            b.Append(ExportString(IMP69_M02_NEPR));
            b.Append(ExportString(IMP69_M03_NEPR));
            b.Append(ExportString(IMP69_M04_NEPR));
            b.Append(ExportString(IMP69_M05_NEPR));
            b.Append(ExportString(IMP69_M06_NEPR));
            b.Append(ExportString(IMP69_M07_NEPR));
            b.Append(ExportString(IMP69_M08_NEPR));
            b.Append(ExportString(IMP69_M09_NEPR));
            b.Append(ExportString(IMP69_M10_NEPR));
            b.Append(ExportString(IMP69_M11_NEPR));
            b.Append(ExportString(IMP69_M12_NEPR));
            b.Append(ExportString(IMP69_M01_ABSE));
            b.Append(ExportString(IMP69_M02_ABSE));
            b.Append(ExportString(IMP69_M03_ABSE));
            b.Append(ExportString(IMP69_M04_ABSE));
            b.Append(ExportString(IMP69_M05_ABSE));
            b.Append(ExportString(IMP69_M06_ABSE));
            b.Append(ExportString(IMP69_M07_ABSE));
            b.Append(ExportString(IMP69_M08_ABSE));
            b.Append(ExportString(IMP69_M09_ABSE));
            b.Append(ExportString(IMP69_M10_ABSE));
            b.Append(ExportString(IMP69_M11_ABSE));
            b.Append(ExportString(IMP69_M12_ABSE));

            return b.ToString();
        }
        public override void ExportToExportFile(StreamWriter writer)
        {
            if (string.IsNullOrEmpty(IMP_OSC))
            {
                return;
            }
            writer.Write("@69||");
            ExportField(writer, IMP_OSC);
            ExportField(writer, IMP_POM);
            ExportField(writer, IMP69_DOV_NROK);
            ExportField(writer, IMP69_DOV_UVAZ);
            ExportField(writer, IMP69_DOV_ZMES);
            ExportField(writer, IMP69_DOB_RZAD);
            ExportField(writer, IMP69_M01_UVAZ);
            ExportField(writer, IMP69_M02_UVAZ);
            ExportField(writer, IMP69_M03_UVAZ);
            ExportField(writer, IMP69_M04_UVAZ);
            ExportField(writer, IMP69_M05_UVAZ);
            ExportField(writer, IMP69_M06_UVAZ);
            ExportField(writer, IMP69_M07_UVAZ);
            ExportField(writer, IMP69_M08_UVAZ);
            ExportField(writer, IMP69_M09_UVAZ);
            ExportField(writer, IMP69_M10_UVAZ);
            ExportField(writer, IMP69_M11_UVAZ);
            ExportField(writer, IMP69_M12_UVAZ);
            ExportField(writer, IMP69_M01_ODPR);
            ExportField(writer, IMP69_M02_ODPR);
            ExportField(writer, IMP69_M03_ODPR);
            ExportField(writer, IMP69_M04_ODPR);
            ExportField(writer, IMP69_M05_ODPR);
            ExportField(writer, IMP69_M06_ODPR);
            ExportField(writer, IMP69_M07_ODPR);
            ExportField(writer, IMP69_M08_ODPR);
            ExportField(writer, IMP69_M09_ODPR);
            ExportField(writer, IMP69_M10_ODPR);
            ExportField(writer, IMP69_M11_ODPR);
            ExportField(writer, IMP69_M12_ODPR);
            ExportField(writer, IMP69_M01_ZAPO);
            ExportField(writer, IMP69_M02_ZAPO);
            ExportField(writer, IMP69_M03_ZAPO);
            ExportField(writer, IMP69_M04_ZAPO);
            ExportField(writer, IMP69_M05_ZAPO);
            ExportField(writer, IMP69_M06_ZAPO);
            ExportField(writer, IMP69_M07_ZAPO);
            ExportField(writer, IMP69_M08_ZAPO);
            ExportField(writer, IMP69_M09_ZAPO);
            ExportField(writer, IMP69_M10_ZAPO);
            ExportField(writer, IMP69_M11_ZAPO);
            ExportField(writer, IMP69_M12_ZAPO);
            ExportField(writer, IMP69_M01_NEPR);
            ExportField(writer, IMP69_M02_NEPR);
            ExportField(writer, IMP69_M03_NEPR);
            ExportField(writer, IMP69_M04_NEPR);
            ExportField(writer, IMP69_M05_NEPR);
            ExportField(writer, IMP69_M06_NEPR);
            ExportField(writer, IMP69_M07_NEPR);
            ExportField(writer, IMP69_M08_NEPR);
            ExportField(writer, IMP69_M09_NEPR);
            ExportField(writer, IMP69_M10_NEPR);
            ExportField(writer, IMP69_M11_NEPR);
            ExportField(writer, IMP69_M12_NEPR);
            ExportField(writer, IMP69_M01_ABSE);
            ExportField(writer, IMP69_M02_ABSE);
            ExportField(writer, IMP69_M03_ABSE);
            ExportField(writer, IMP69_M04_ABSE);
            ExportField(writer, IMP69_M05_ABSE);
            ExportField(writer, IMP69_M06_ABSE);
            ExportField(writer, IMP69_M07_ABSE);
            ExportField(writer, IMP69_M08_ABSE);
            ExportField(writer, IMP69_M09_ABSE);
            ExportField(writer, IMP69_M10_ABSE);
            ExportField(writer, IMP69_M11_ABSE);
            ExportField(writer, IMP69_M12_ABSE);

            writer.WriteLine();

        }
    }
    public class ImportData80 : ImportData
    {
        public string IMP_OSC;
        public string IMP_POM;
        public string IMP80_DOV_NROK;
        public string IMP80_DOV_NMES;
        public string IMP80_DOV_PREZ;
        public string IMP80_DOV_UVAZ;
        public string IMP80_M01_SMEN;
        public string IMP80_M02_SMEN;
        public string IMP80_M03_SMEN;
        public string IMP80_M04_SMEN;
        public string IMP80_M05_SMEN;
        public string IMP80_M06_SMEN;
        public string IMP80_M07_SMEN;
        public string IMP80_M08_SMEN;
        public string IMP80_M09_SMEN;
        public string IMP80_M10_SMEN;
        public string IMP80_M11_SMEN;
        public string IMP80_M12_SMEN;

        public ImportData80()
        {
            IMP_OSC = "";
            IMP_POM = "";
            IMP80_DOV_NROK = "#";
            IMP80_DOV_NMES = "#";
            IMP80_DOV_PREZ = "#";
            IMP80_DOV_UVAZ = "#";
            IMP80_M01_SMEN = "#";
            IMP80_M02_SMEN = "#";
            IMP80_M03_SMEN = "#";
            IMP80_M04_SMEN = "#";
            IMP80_M05_SMEN = "#";
            IMP80_M06_SMEN = "#";
            IMP80_M07_SMEN = "#";
            IMP80_M08_SMEN = "#";
            IMP80_M09_SMEN = "#";
            IMP80_M10_SMEN = "#";
            IMP80_M11_SMEN = "#";
            IMP80_M12_SMEN = "#";
        }
        public override string Export()
        {
            StringBuilder b = new StringBuilder("@80||");
            b.Append(ExportString(IMP_OSC));
            b.Append(ExportString(IMP_POM));
            b.Append(ExportString(IMP80_DOV_NROK));
            b.Append(ExportString(IMP80_DOV_NMES));
            b.Append(ExportString(IMP80_DOV_PREZ));
            b.Append(ExportString(IMP80_DOV_UVAZ));
            b.Append(ExportString(IMP80_M01_SMEN));
            b.Append(ExportString(IMP80_M02_SMEN));
            b.Append(ExportString(IMP80_M03_SMEN));
            b.Append(ExportString(IMP80_M04_SMEN));
            b.Append(ExportString(IMP80_M05_SMEN));
            b.Append(ExportString(IMP80_M06_SMEN));
            b.Append(ExportString(IMP80_M07_SMEN));
            b.Append(ExportString(IMP80_M08_SMEN));
            b.Append(ExportString(IMP80_M09_SMEN));
            b.Append(ExportString(IMP80_M10_SMEN));
            b.Append(ExportString(IMP80_M11_SMEN));
            b.Append(ExportString(IMP80_M12_SMEN));

            return b.ToString();
        }
        public override void ExportToExportFile(StreamWriter writer)
        {
            if (string.IsNullOrEmpty(IMP_OSC))
            {
                return;
            }
            writer.Write("@80||");
            ExportField(writer, IMP_OSC);
            ExportField(writer, IMP_POM);
            ExportField(writer, IMP80_DOV_NROK);
            ExportField(writer, IMP80_DOV_NMES);
            ExportField(writer, IMP80_DOV_PREZ);
            ExportField(writer, IMP80_DOV_UVAZ);
            ExportField(writer, IMP80_M01_SMEN);
            ExportField(writer, IMP80_M02_SMEN);
            ExportField(writer, IMP80_M03_SMEN);
            ExportField(writer, IMP80_M04_SMEN);
            ExportField(writer, IMP80_M05_SMEN);
            ExportField(writer, IMP80_M06_SMEN);
            ExportField(writer, IMP80_M07_SMEN);
            ExportField(writer, IMP80_M08_SMEN);
            ExportField(writer, IMP80_M09_SMEN);
            ExportField(writer, IMP80_M10_SMEN);
            ExportField(writer, IMP80_M11_SMEN);
            ExportField(writer, IMP80_M12_SMEN);

            writer.WriteLine();

        }
    }
    public class ImportData24 : ImportData
    {
        public string IMP_OSC;
        public string IMP_POM;
        public string IMP24_OBDOBIMES;
        public string IMP24_OBDOBIROK;
        public string IMP24_ZAPVYDDNY;
        public string IMP24_ZAPVYDNED;
        public string IMP24_ZAPVYDKCS;
        public string IMP24_VYMZAKSOC;
        public string IMP24_VYMZAKZDR;
        public string IMP24_VYMZAKKOP;
        public string IMP24_OPERATION;

        public ImportData24()
        {
            IMP_OSC = "";
            IMP_POM = "";
            IMP24_OBDOBIMES = "#";
            IMP24_OBDOBIROK = "#";
            IMP24_ZAPVYDDNY = "#";
            IMP24_ZAPVYDNED = "#";
            IMP24_ZAPVYDKCS = "#";
            IMP24_VYMZAKSOC = "#";
            IMP24_VYMZAKZDR = "#";
            IMP24_VYMZAKKOP = "#";
            IMP24_OPERATION = "#";
        }
        public override string Export()
        {
            StringBuilder b = new StringBuilder("@24||");
            b.Append(ExportString(IMP_OSC));
            b.Append(ExportString(IMP_POM));
            b.Append(ExportString(IMP24_OBDOBIMES));
            b.Append(ExportString(IMP24_OBDOBIROK));
            b.Append(ExportString(IMP24_ZAPVYDDNY));
            b.Append(ExportString(IMP24_ZAPVYDNED));
            b.Append(ExportString(IMP24_ZAPVYDKCS));
            b.Append(ExportString(IMP24_VYMZAKSOC));
            b.Append(ExportString(IMP24_VYMZAKZDR));
            b.Append(ExportString(IMP24_VYMZAKKOP));
            b.Append(ExportString(IMP24_OPERATION));

            return b.ToString();
        }
        public override void ExportToExportFile(StreamWriter writer)
        {
            if (string.IsNullOrEmpty(IMP_OSC))
            {
                return;
            }
            writer.Write("@24||");
            ExportField(writer, IMP_OSC);
            ExportField(writer, IMP_POM);
            ExportField(writer, IMP24_OBDOBIMES);
            ExportField(writer, IMP24_OBDOBIROK);
            ExportField(writer, IMP24_ZAPVYDDNY);
            ExportField(writer, IMP24_ZAPVYDNED);
            ExportField(writer, IMP24_ZAPVYDKCS);
            ExportField(writer, IMP24_VYMZAKSOC);
            ExportField(writer, IMP24_VYMZAKZDR);
            ExportField(writer, IMP24_VYMZAKKOP);
            ExportField(writer, IMP24_OPERATION);

            writer.WriteLine();

        }
    }
    public class ImportData64 : ImportData
    {
        public string IMP_OSC;
        public string IMP_POM;
        public string IMP64_OBDOBIMES;
        public string IMP64_OBDOBIROK;
        public string IMP64_VYMZAKSOC;
        public string IMP64_VYMZAKZDR;
        public string IMP64_VYMZAKPEN;
        public string IMP64_VYMZAKTYP;
        public string IMP64_OPERATION;

        public ImportData64()
        {
            IMP_OSC = "";
            IMP_POM = "";
            IMP64_OBDOBIMES = "#";
            IMP64_OBDOBIROK = "#";
            IMP64_VYMZAKSOC = "#";
            IMP64_VYMZAKZDR = "#";
            IMP64_VYMZAKPEN = "#";
            IMP64_VYMZAKTYP = "#";
            IMP64_OPERATION = "#";
        }
        public override string Export()
        {
            StringBuilder b = new StringBuilder("@64||");
            b.Append(ExportString(IMP_OSC));
            b.Append(ExportString(IMP_POM));
            b.Append(ExportString(IMP64_OBDOBIMES));
            b.Append(ExportString(IMP64_OBDOBIROK));
            b.Append(ExportString(IMP64_VYMZAKSOC));
            b.Append(ExportString(IMP64_VYMZAKZDR));
            b.Append(ExportString(IMP64_VYMZAKPEN));
            b.Append(ExportString(IMP64_VYMZAKTYP));
            b.Append(ExportString(IMP64_OPERATION));

            return b.ToString();
        }
        public override void ExportToExportFile(StreamWriter writer)
        {
            if (string.IsNullOrEmpty(IMP_OSC))
            {
                return;
            }
            writer.Write("@64||");
            ExportField(writer, IMP_OSC);
            ExportField(writer, IMP_POM);
            ExportField(writer, IMP64_OBDOBIMES);
            ExportField(writer, IMP64_OBDOBIROK);
            ExportField(writer, IMP64_VYMZAKSOC);
            ExportField(writer, IMP64_VYMZAKZDR);
            ExportField(writer, IMP64_VYMZAKPEN);
            ExportField(writer, IMP64_VYMZAKTYP);
            ExportField(writer, IMP64_OPERATION);

            writer.WriteLine();
        }
    }
    public class ImportData44 : ImportData
    {
        public string IMP_OSC;
        public string IMP_POM;
        public string IMP44_KODNEPR;
        public string IMP44_KODNEPRTEXT;
        public string IMP44_DATUMZAC;
        public string IMP44_DATUMKON;
        public string IMP44_POCETDNU;
        public string IMP44_VYLOUCENA;
        public string IMP44_DOKLAD;
        public string IMP44_NAZEVZAM;
        public string IMP44_ICZAM;
        public string IMP44_DATPOROD;
        public string IMP44_HODNOTAKC;

        public ImportData44()
        {
            IMP_OSC = "";
            IMP_POM = "";
            IMP44_KODNEPR = "#";
            IMP44_KODNEPRTEXT = "#";
            IMP44_DATUMZAC = "#";
            IMP44_DATUMKON = "#";
            IMP44_POCETDNU = "#";
            IMP44_VYLOUCENA = "#";
            IMP44_DOKLAD = "#";
            IMP44_NAZEVZAM = "#";
            IMP44_ICZAM = "#";
            IMP44_DATPOROD = "#";
            IMP44_HODNOTAKC = "#";
        }
        public override string Export()
        {
            StringBuilder b = new StringBuilder("@44||");
            b.Append(ExportString(IMP_OSC));
            b.Append(ExportString(IMP_POM));
            b.Append(ExportString(IMP44_KODNEPR));
            b.Append(ExportString(IMP44_KODNEPRTEXT));
            b.Append(ExportString(IMP44_DATUMZAC));
            b.Append(ExportString(IMP44_DATUMKON));
            b.Append(ExportString(IMP44_POCETDNU));
            b.Append(ExportString(IMP44_VYLOUCENA));
            b.Append(ExportString(IMP44_DOKLAD));
            b.Append(ExportString(IMP44_NAZEVZAM));
            b.Append(ExportString(IMP44_ICZAM));
            b.Append(ExportString(IMP44_DATPOROD));
            b.Append(ExportString(IMP44_HODNOTAKC));

            return b.ToString();
        }
        public override void ExportToExportFile(StreamWriter writer)
        {
            if (string.IsNullOrEmpty(IMP_OSC))
            {
                return;
            }
            writer.Write("@44||");
            ExportField(writer, IMP_OSC);
            ExportField(writer, IMP_POM);
            ExportField(writer, IMP44_KODNEPR);
            ExportField(writer, IMP44_KODNEPRTEXT);
            ExportField(writer, IMP44_DATUMZAC);
            ExportField(writer, IMP44_DATUMKON);
            ExportField(writer, IMP44_POCETDNU);
            ExportField(writer, IMP44_VYLOUCENA);
            ExportField(writer, IMP44_DOKLAD);
            ExportField(writer, IMP44_NAZEVZAM);
            ExportField(writer, IMP44_ICZAM);
            ExportField(writer, IMP44_DATPOROD);
            ExportField(writer, IMP44_HODNOTAKC);

            writer.WriteLine();
        }
    }
    public class ImportData19 : ImportData
    {
        public string IMP_OSC;
        public string IMP_POM;
        public string IMP19_KODMZDA;
        public string IMP19_KODMZDATEXT;
        public string IMP19_MINUTY;
        public string IMP19_MINUTYNORM;
        public string IMP19_JEDNOTKY;
        public string IMP19_SAZBAKC;
        public string IMP19_SAZBAPROC;
        public string IMP19_STRED;
        public string IMP19_CINN;
        public string IMP19_ZAK;
        public string IMP19_TRVALE;
        public string IMP19_OPT_TARIF;
        public string IMP19_OPT_TSTUP;
        public string IMP19_OPT_TAUTO;
        public string IMP19_OPT_ZDROJ;
        public string IMP19_VALUTA_KOD;

        public ImportData19()
        {
            IMP_OSC = "";
            IMP_POM = "";
            IMP19_KODMZDA = "#";
            IMP19_KODMZDATEXT = "#";
            IMP19_MINUTY = "#";
            IMP19_MINUTYNORM = "#";
            IMP19_JEDNOTKY = "#";
            IMP19_SAZBAKC = "#";
            IMP19_SAZBAPROC = "#";
            IMP19_STRED = "#";
            IMP19_CINN = "#";
            IMP19_ZAK = "#";
            IMP19_TRVALE = "#";
            IMP19_OPT_TARIF = "#";
            IMP19_OPT_TSTUP = "#";
            IMP19_OPT_TAUTO = "#";
            IMP19_OPT_ZDROJ = "#";
            IMP19_VALUTA_KOD = "#";
        }
        public override string Export()
        {
            StringBuilder b = new StringBuilder("@19||");
            b.Append(ExportString(IMP_OSC));
            b.Append(ExportString(IMP_POM));
            b.Append(ExportString(IMP19_KODMZDA));
            b.Append(ExportString(IMP19_KODMZDATEXT));
            b.Append(ExportString(IMP19_MINUTY));
            b.Append(ExportString(IMP19_MINUTYNORM));
            b.Append(ExportString(IMP19_JEDNOTKY));
            b.Append(ExportString(IMP19_SAZBAKC));
            b.Append(ExportString(IMP19_SAZBAPROC));
            b.Append(ExportString(IMP19_STRED));
            b.Append(ExportString(IMP19_CINN));
            b.Append(ExportString(IMP19_ZAK));
            b.Append(ExportString(IMP19_TRVALE));
            b.Append(ExportString(IMP19_OPT_TARIF));
            b.Append(ExportString(IMP19_OPT_TSTUP));
            b.Append(ExportString(IMP19_OPT_TAUTO));
            b.Append(ExportString(IMP19_OPT_ZDROJ));
            b.Append(ExportString(IMP19_VALUTA_KOD));

            return b.ToString();
        }
        public override void ExportToExportFile(StreamWriter writer)
        {
            if (string.IsNullOrEmpty(IMP_OSC))
            {
                return;
            }
            writer.Write("@19||");
            ExportField(writer, IMP_OSC);
            ExportField(writer, IMP_POM);
            ExportField(writer, IMP19_KODMZDA);
            ExportField(writer, IMP19_KODMZDATEXT);
            ExportField(writer, IMP19_MINUTY);
            ExportField(writer, IMP19_MINUTYNORM);
            ExportField(writer, IMP19_JEDNOTKY);
            ExportField(writer, IMP19_SAZBAKC);
            ExportField(writer, IMP19_SAZBAPROC);
            ExportField(writer, IMP19_STRED);
            ExportField(writer, IMP19_CINN);
            ExportField(writer, IMP19_ZAK);
            ExportField(writer, IMP19_TRVALE);
            ExportField(writer, IMP19_OPT_TARIF);
            ExportField(writer, IMP19_OPT_TSTUP);
            ExportField(writer, IMP19_OPT_TAUTO);
            ExportField(writer, IMP19_OPT_ZDROJ);
            ExportField(writer, IMP19_VALUTA_KOD);

            writer.WriteLine();

        }
    }
    public class ImportData21 : ImportData
    {
        public string IMP_OSC;
        public string IMP21_KODSRAZ;
        public string IMP21_KODSRAZTEXT;
        public string IMP21_CELKEM;
        public string IMP21_MESICNI;
        public string IMP21_PROCJED;
        public string IMP21_POSLEDNI;
        public string IMP21_POSLEDNIDEN;
        public string IMP21_POSLEDNIMES;
        public string IMP21_POSLEDNIROK;
        public string IMP21_STRED;
        public string IMP21_CINN;
        public string IMP21_ZAK;
        public string IMP21_TRVALE;
        public string IMP21_ZPUSOB;
        public string IMP21_OBEC;
        public string IMP21_ADRESA_COBEC;
        public string IMP21_ADRESA_DCIS;
        public string IMP21_ADRESA_DTYP;
        public string IMP21_ADRESA_ULICE;
        public string IMP21_ADRESA_PSC;
        public string IMP21_ADRESA_POSTA;
        public string IMP21_ADRESA_OCIS;
        public string IMP21_UCET;
        public string IMP21_BKSPOJ_USTAV;
        public string IMP21_BKSPOJ_KSYMB;
        public string IMP21_BKSPOJ_VSYMB;
        public string IMP21_BKSPOJ_SSYMB;
        public string IMP21_OPT_MENA;
        public string IMP21_OPT_ZEME;
        public string IMP21_OPT_MESTO;
        public string IMP21_SRAZJENPLNA;
        public string IMP21_OSR_PORADI;
        public string IMP21_OSR_POZNAM;

        public ImportData21()
        {
            IMP_OSC = "";
            IMP21_KODSRAZ = "#";
            IMP21_KODSRAZTEXT = "#";
            IMP21_CELKEM = "#";
            IMP21_MESICNI = "#";
            IMP21_PROCJED = "#";
            IMP21_POSLEDNI = "#";
            IMP21_POSLEDNIDEN = "#";
            IMP21_POSLEDNIMES = "#";
            IMP21_POSLEDNIROK = "#";
            IMP21_STRED = "#";
            IMP21_CINN = "#";
            IMP21_ZAK = "#";
            IMP21_TRVALE = "#";
            IMP21_ZPUSOB = "#";
            IMP21_OBEC = "#";
            IMP21_ADRESA_COBEC = "#";
            IMP21_ADRESA_DCIS = "#";
            IMP21_ADRESA_DTYP = "#";
            IMP21_ADRESA_ULICE = "#";
            IMP21_ADRESA_PSC = "#";
            IMP21_ADRESA_POSTA = "#";
            IMP21_ADRESA_OCIS = "#";
            IMP21_UCET = "#";
            IMP21_BKSPOJ_USTAV = "#";
            IMP21_BKSPOJ_KSYMB = "#";
            IMP21_BKSPOJ_VSYMB = "#";
            IMP21_BKSPOJ_SSYMB = "#";
            IMP21_OPT_MENA = "#";
            IMP21_OPT_ZEME = "#";
            IMP21_OPT_MESTO = "#";
            IMP21_SRAZJENPLNA = "#";
            IMP21_OSR_PORADI = "#";
            IMP21_OSR_POZNAM = "#";
        }
        public override string Export()
        {
            StringBuilder b = new StringBuilder("@21||");
            b.Append(ExportString(IMP_OSC));
            b.Append(ExportString(IMP21_KODSRAZ));
            b.Append(ExportString(IMP21_KODSRAZTEXT));
            b.Append(ExportString(IMP21_CELKEM));
            b.Append(ExportString(IMP21_MESICNI));
            b.Append(ExportString(IMP21_PROCJED));
            b.Append(ExportString(IMP21_POSLEDNI));
            b.Append(ExportString(IMP21_POSLEDNIDEN));
            b.Append(ExportString(IMP21_POSLEDNIMES));
            b.Append(ExportString(IMP21_POSLEDNIROK));
            b.Append(ExportString(IMP21_STRED));
            b.Append(ExportString(IMP21_CINN));
            b.Append(ExportString(IMP21_ZAK));
            b.Append(ExportString(IMP21_TRVALE));
            b.Append(ExportString(IMP21_ZPUSOB));
            b.Append(ExportString(IMP21_OBEC));
            b.Append(ExportString(IMP21_ADRESA_COBEC));
            b.Append(ExportString(IMP21_ADRESA_DCIS));
            b.Append(ExportString(IMP21_ADRESA_DTYP));
            b.Append(ExportString(IMP21_ADRESA_ULICE));
            b.Append(ExportString(IMP21_ADRESA_PSC));
            b.Append(ExportString(IMP21_ADRESA_POSTA));
            b.Append(ExportString(IMP21_ADRESA_OCIS));
            b.Append(ExportString(IMP21_UCET));
            b.Append(ExportString(IMP21_BKSPOJ_USTAV));
            b.Append(ExportString(IMP21_BKSPOJ_KSYMB));
            b.Append(ExportString(IMP21_BKSPOJ_VSYMB));
            b.Append(ExportString(IMP21_BKSPOJ_SSYMB));
            b.Append(ExportString(IMP21_OPT_MENA));
            b.Append(ExportString(IMP21_OPT_ZEME));
            b.Append(ExportString(IMP21_OPT_MESTO));
            b.Append(ExportString(IMP21_SRAZJENPLNA));
            b.Append(ExportString(IMP21_OSR_PORADI));
            b.Append(ExportString(IMP21_OSR_POZNAM));

            return b.ToString();
        }
        public override void ExportToExportFile(StreamWriter writer)
        {
            if (string.IsNullOrEmpty(IMP_OSC))
            {
                return;
            }
            writer.Write("@21||");
            ExportField(writer, IMP_OSC);
            ExportField(writer, IMP21_KODSRAZ);
            ExportField(writer, IMP21_KODSRAZTEXT);
            ExportField(writer, IMP21_CELKEM);
            ExportField(writer, IMP21_MESICNI);
            ExportField(writer, IMP21_PROCJED);
            ExportField(writer, IMP21_POSLEDNI);
            ExportField(writer, IMP21_POSLEDNIDEN);
            ExportField(writer, IMP21_POSLEDNIMES);
            ExportField(writer, IMP21_POSLEDNIROK);
            ExportField(writer, IMP21_STRED);
            ExportField(writer, IMP21_CINN);
            ExportField(writer, IMP21_ZAK);
            ExportField(writer, IMP21_TRVALE);
            ExportField(writer, IMP21_ZPUSOB);
            ExportField(writer, IMP21_OBEC);
            ExportField(writer, IMP21_ADRESA_COBEC);
            ExportField(writer, IMP21_ADRESA_DCIS);
            ExportField(writer, IMP21_ADRESA_DTYP);
            ExportField(writer, IMP21_ADRESA_ULICE);
            ExportField(writer, IMP21_ADRESA_PSC);
            ExportField(writer, IMP21_ADRESA_POSTA);
            ExportField(writer, IMP21_ADRESA_OCIS);
            ExportField(writer, IMP21_UCET);
            ExportField(writer, IMP21_BKSPOJ_USTAV);
            ExportField(writer, IMP21_BKSPOJ_KSYMB);
            ExportField(writer, IMP21_BKSPOJ_VSYMB);
            ExportField(writer, IMP21_BKSPOJ_SSYMB);
            ExportField(writer, IMP21_OPT_MENA);
            ExportField(writer, IMP21_OPT_ZEME);
            ExportField(writer, IMP21_OPT_MESTO);
            ExportField(writer, IMP21_SRAZJENPLNA);
            ExportField(writer, IMP21_OSR_PORADI);
            ExportField(writer, IMP21_OSR_POZNAM);

            writer.WriteLine();
        }
    }
}
