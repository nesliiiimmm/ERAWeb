using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AYAK.Common.NetCore
{
    public static class StringTools
    {
        #region Metin İşleri
        public static string AralikGetir(this string text, int bas, int bit)
        {
            if (text == null) return null;
            if (text == string.Empty) return string.Empty;
            if (bas == -1 || bit == -1) return null;

            return text.Substring(bas, bit);
        }
        public static string AralikGetir(this string text, string bas, string bit)
        {
            int bbas = 0;
            string r = AralikGetir(text, bas, bit, out bbas);
            return r;
        }
        public static string AralikGetir(this string text, string bul, string baslangic, string bitis)
        {
            string res = "";
            int i = text.IndexOf(bul);
            i = text.IndexOf(baslangic, i + bul.Length);
            int e = text.IndexOf(bitis, i + baslangic.Length);
            res = text.Substring(i + baslangic.Length, e - i - baslangic.Length);
            return res;
        }
        public static string AralikGetir(this string text, string bas, string bit, bool basBitGetir)
        {
            int bbas = 0;
            return AralikGetir(text, bas, bit, 0, basBitGetir, out bbas);
        }
        public static string AralikGetir(this string text, string bas, string bit, out int bulunanBaslangic)
        {
            string r = AralikGetir(text, bas, bit, 0, out bulunanBaslangic);
            return r;
        }
        public static string AralikGetir(this string text, string bas, string bit, int aramaBas, bool basbitDahil)
        {
            int bbas = 0;
            return AralikGetir(text, bas, bit, aramaBas, basbitDahil, out bbas);
        }
        public static string AralikGetir(this string text, string bas, string bit, int aramaBas)
        {
            int bbas = 0;
            string r = AralikGetir(text, bas, bit, aramaBas, out bbas);
            return r;
        }

        public static string AralikGetir(this string text, string bas, string bit, int aramaBas, out int bulunanBaslangic)
        {
            return AralikGetir(text, bas, bit, aramaBas, true, out bulunanBaslangic);
        }

        public static string AralikGetir(this string text, string bas, string bit, int aramaBas, bool basBitdahil, out int bulunanBaslangic)
        {
            int a = text.IndexOf(bas, aramaBas + 1);
            string r = "";
            if (a != -1)
            {
                int b = text.IndexOf(bit, a + 1);
                if (bas.Contains(bit))
                {
                    b = text.IndexOf(bit, a + bas.Length);
                }
                if (b != -1)
                {
                    if (basBitdahil)
                    {
                        r = text.Substring(a, b - a + bit.Length);
                    }
                    else
                    {
                        r = text.Substring(a + bas.Length, b - a - bas.Length);
                    }
                }
                else
                {
                    r = text.Substring(basBitdahil ? a : a + bas.Length);
                }

            }

            bulunanBaslangic = a;
            return r;
        }
        public static int GeriyeDogruBak(this string kaynak, int baslangic, string metin)
        {
            string bul = "";
            do
            {
                bul = kaynak.Substring(baslangic, metin.Length);
                baslangic--;

            } while (bul != metin && baslangic >= 0);
            return baslangic;

        }

        public static string BuyukBosluklariSil(this string text, int adet = 100)
        {
            if (string.IsNullOrEmpty(text?.Trim())) return text;
            for (int i = adet; i > 1; i--)
            {
                string bosluklu = "";
                for (int j = 0; j < i; j++)
                {
                    bosluklu += " ";
                }
                text = text.Replace(bosluklu, "");
            }
            return text;
        }

        #endregion
    }
}
