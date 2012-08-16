using System;
using System.Collections.Generic;
using TxtFileGenerator;

namespace Test
{
    public enum Movimentacao { ComDados = 0, SemDados = 1 }

    public enum IndicadorCentralizacao { Matriz = 0, Filial = 1 }

    public enum SituacaoEspecial { Abertura = 0, Cisao = 1, Fusao = 2, Incorporacao = 3, Extincao = 4, Nenhuma = 5 }

    public static class ConstantesSped
    {
        public const string Sintetica = "S";
        public const string Analitica = "A";
        public const string Devedor = "D";
        public const string Credor = "C";
        public const string LancamentoNormal = "N";
        public const string LancamentoEncerramento = "E";
    }

    public abstract class LeiauteSped
    {
        [TxtIgnoreProperty]
        public int QdeDeLinhas { get; set; }

        public abstract string ToString();

        protected LeiauteSped()
        {
            QdeDeLinhas = 0;
        }
    }

    [TxtRegisterName("0000")]
    public class SpedRegistro0000 : LeiauteSped
    {
        public DateTime DT_INI { get; set; }
        public DateTime DT_FIN { get; set; }
        public string NOME { get; set; }
        public string CNPJ { get; set; }
        public string UF { get; set; }
        public string IE { get; set; }
        public string COD_MUN { get; set; }
        public string IM { get; set; }
        public SituacaoEspecial IND_SIT_ESP { get; set; }

        public override string ToString()
        {
            QdeDeLinhas = 1;
            return "|0000|LECD|" +
                    DT_INI.FormataParaSPED() +
                    DT_FIN.FormataParaSPED() +
                    NOME.FormataParaSPED() +
                    CNPJ.FormataParaSPED() +
                    UF.FormataParaSPED() +
                    IE.FormataParaSPED() +
                    COD_MUN.FormataParaSPED() +
                    IM.FormataParaSPED() +
                    IND_SIT_ESP.FormataParaSPED().FimDaLinha();
        }

    }

    [TxtRegisterName("0001")]
    public class SpedRegistro0001 : LeiauteSped
    {
        public Movimentacao IND_DAD { get; set; }

        public override string ToString()
        {
            QdeDeLinhas = 1;
            return "|0001|" + IND_DAD.FormataParaSPED().FimDaLinha();
        }
    }

    [TxtRegisterName("0007")]
    public class SpedRegistro0007 : LeiauteSped
    {
        public string COD_ENT_REF { get; set; }
        public string COD_INSCR { get; set; }
        public override string ToString()
        {
            QdeDeLinhas = 1;
            return "|0007|" +
                   COD_ENT_REF.FormataParaSPED() +
                   COD_INSCR.FormataParaSPED().FimDaLinha();
        }
    }

    [TxtRegisterName("0020")]
    public class SpedRegistro0020 : LeiauteSped
    {
        [TxtPropertyTypeValue(typeof(string))]
        public IndicadorCentralizacao IND_DEC { get; set; }
        public string CNPJ { get; set; }
        public string UF { get; set; }
        public string IE { get; set; }
        public string COD_MUN { get; set; }
        public string IM { get; set; }
        public string NIER { get; set; }
        public override string ToString()
        {
            QdeDeLinhas = 1;
            return "|0020|" +
                    IND_DEC.ToString().FormataParaSPED() +
                    CNPJ.FormataParaSPED() +
                    UF.FormataParaSPED() +
                    IE.FormataParaSPED() +
                    COD_MUN.FormataParaSPED() +
                    IM.FormataParaSPED() +
                    NIER.FormataParaSPED().FimDaLinha();
        }
    }

    [TxtRegisterName("0990")]
    public class SpedRegistro0990 : LeiauteSped
    {
        public override string ToString()
        {
            QdeDeLinhas = 1;
            return "|0990|" + QdeDeLinhas.FormataParaSPED().FimDaLinha();
        }
    }

    [TxtRegisterName("I001")]
    public class SpedRegistroI001 : LeiauteSped
    {
        private Movimentacao _IND_DAD;
        public Movimentacao IND_DAD
        {
            get { return _IND_DAD; }
        }

        public SpedRegistroI010 RegI010 { get; set; }

        public SpedRegistroI001()
        {
            RegI010 = new SpedRegistroI010();
        }

        public override string ToString()
        {
            QdeDeLinhas = 1;
            var textoFilhos = RegI010.ToString();
            _IND_DAD = Movimentacao.SemDados;
            if (RegI010.QdeDeLinhas > 0)
                _IND_DAD = Movimentacao.ComDados;
            QdeDeLinhas += RegI010.QdeDeLinhas;
            return "|I001|" +
                   IND_DAD.FormataParaSPED().FimDaLinha() +
                   textoFilhos;
        }
    }

    [TxtRegisterName("I010")]
    public class SpedRegistroI010 : LeiauteSped
    {
        public SpedRegistroI030 RegI030 { get; set; }
        public List<SpedRegistroI050> RegI050 { get; set; }
        public List<SpedRegistroI075> RegI075 { get; set; }
        public List<SpedRegistroI150> RegI150 { get; set; }
        public List<SpedRegistroI200> RegI200 { get; set; }
        public List<SpedRegistroI350> RegI350 { get; set; }

        public SpedRegistroI010()
        {
            RegI030 = new SpedRegistroI030();
            RegI050 = new List<SpedRegistroI050>();
            RegI075 = new List<SpedRegistroI075>();
            RegI150 = new List<SpedRegistroI150>();
            RegI200 = new List<SpedRegistroI200>();
            RegI350 = new List<SpedRegistroI350>();
        }

        public override string ToString()
        {
            string resultado = "";
            resultado += RegI030.ToString();
            QdeDeLinhas += RegI030.QdeDeLinhas;

            foreach (var r in RegI050)
            {
                resultado += r.ToString();
                QdeDeLinhas += r.QdeDeLinhas;
            }
            foreach (var r in RegI075)
            {
                resultado += r.ToString();
                QdeDeLinhas += r.QdeDeLinhas;
            }

            foreach (var r in RegI150)
            {
                resultado += r.ToString();
                QdeDeLinhas += r.QdeDeLinhas;
            }

            foreach (var r in RegI200)
            {
                resultado += r.ToString();
                QdeDeLinhas += r.QdeDeLinhas;
            }

            foreach (var r in RegI350)
            {
                resultado += r.ToString();
                QdeDeLinhas += r.QdeDeLinhas;
            }

            if (QdeDeLinhas > 1)
            {
                QdeDeLinhas++;
                resultado = "|I010|G|".FimDaLinha() + resultado;
            }
            return resultado;
        }
    }

    [TxtRegisterName("I030")]
    public class SpedRegistroI030 : LeiauteSped
    {
        private int _QTD_LIN;
        public int QdeLinhaOutrosRegistros { get; set; }

        public string DNRC_ABERT { get; set; }
        public string NUM_ORD { get; set; }
        public string NAT_LIVR { get; set; }
        public int QTD_LIN { get { return _QTD_LIN; } } //ver
        public string Nome { get; set; }
        public string NIRE { get; set; }
        public string CNPJ { get; set; }
        public DateTime? DT_ARQ { get; set; }
        public DateTime? DT_ARQ_CONV { get; set; }
        public string DESC_MUN { get; set; }

        public SpedRegistroI030()
        {
            QdeLinhaOutrosRegistros = 0;
        }

        public override string ToString()
        {
            QdeDeLinhas = 1;
            _QTD_LIN = QdeLinhaOutrosRegistros + 1;
            return "|I030|" +
                   DNRC_ABERT.FormataParaSPED() +
                   NUM_ORD.FormataParaSPED() +
                   NAT_LIVR.FormataParaSPED() +
                   QTD_LIN.FormataParaSPED() +
                   Nome.FormataParaSPED() +
                   NIRE.FormataParaSPED() +
                   CNPJ.FormataParaSPED() +
                   DT_ARQ.FormataParaSPED() +
                   DT_ARQ_CONV.FormataParaSPED() +
                   DESC_MUN.FormataParaSPED().FimDaLinha();
        }
    }

    [TxtRegisterName("I050")]
    public class SpedRegistroI050 : LeiauteSped
    {
        public DateTime DT_ALT { get; set; }
        public string COD_NAT { get; set; }
        public string IND_CTA { get; set; }
        public int Nivel { get; set; }
        public string COD_CTA { get; set; }
        public string COD_CTA_SUP { get; set; }
        public string CTA { get; set; }

        public SpedRegistroI051 RegI051 { get; set; }
        public List<SpedRegistroI052> RegI052 { get; set; }

        public SpedRegistroI050()
        {
            RegI052 = new List<SpedRegistroI052>();
        }

        public override string ToString()
        {
            string Resultado = "|I050|" +
                                DT_ALT.FormataParaSPED() +
                                COD_NAT.FormataParaSPED() +
                                IND_CTA.FormataParaSPED() +
                                Nivel.FormataParaSPED() +
                                COD_CTA.FormataParaSPED() +
                                COD_CTA_SUP.FormataParaSPED() +
                                CTA.FormataParaSPED().FimDaLinha();
            QdeDeLinhas = 1;
            if (RegI051 != null)
            {
                Resultado += RegI051.ToString();
                QdeDeLinhas += RegI051.QdeDeLinhas;
            }

            foreach (var r in RegI052)
            {
                Resultado += r.ToString();
                QdeDeLinhas += r.QdeDeLinhas;
            }

            return Resultado;
        }
    }

    // Plano de Contas Referencial
    [TxtRegisterName("I051")]
    public class SpedRegistroI051 : LeiauteSped
    {
        public string COD_ENT_REF { get; set; }
        public string COD_CCUS { get; set; }
        public string COD_CTA_REF { get; set; }
        public override string ToString()
        {
            QdeDeLinhas = 1;
            return "|I051|" +
                   COD_ENT_REF.FormataParaSPED() +
                   COD_CCUS.FormataParaSPED() +
                   COD_CTA_REF.FormataParaSPED().FimDaLinha();
        }
    }

    [TxtRegisterName("I052")]
    public class SpedRegistroI052 : LeiauteSped
    {
        public string COD_CCUS { get; set; }
        public string COD_AGL { get; set; }
        public override string ToString()
        {
            QdeDeLinhas = 1;
            return "|I052|" +
                   COD_CCUS.FormataParaSPED() +
                   COD_AGL.FormataParaSPED().FimDaLinha();
        }
    }

    [TxtRegisterName("I075")]
    public class SpedRegistroI075 : LeiauteSped
    {
        public string COD_HIST { get; set; }
        public string DESCR_HIST { get; set; }
        public override string ToString()
        {
            QdeDeLinhas = 1;
            return "|I075|" +
                   COD_HIST.FormataParaSPED() +
                   DESCR_HIST.FormataParaSPED().FimDaLinha();
        }
    }

    [TxtRegisterName("I150")]
    public class SpedRegistroI150 : LeiauteSped
    {
        public DateTime? DT_INI { get; set; }
        public DateTime? DT_FIN { get; set; }

        public List<SpedRegistroI155> RegI155 { get; set; }

        public SpedRegistroI150()
        {
            RegI155 = new List<SpedRegistroI155>();
        }

        public override string ToString()
        {
            string Resultado = "|I150|" +
                   DT_INI.FormataParaSPED() +
                   DT_FIN.FormataParaSPED().FimDaLinha();
            QdeDeLinhas = 1;
            foreach (var r in RegI155)
            {
                Resultado += r.ToString();
                QdeDeLinhas += r.QdeDeLinhas;
            }

            return Resultado;
        }
    }

    [TxtRegisterName("I155")]
    public class SpedRegistroI155 : LeiauteSped
    {
        public string COD_CTA { get; set; }
        public string COD_CCUS { get; set; }
        public Double VL_SLD_INI { get; set; }
        public string IND_DC_INI { get; set; }
        public Double VL_DEB { get; set; }
        public Double VL_CRED { get; set; }
        public Double VL_SLD_FIN { get; set; }
        public string IND_DC_FIN { get; set; }

        public override string ToString()
        {
            QdeDeLinhas = 1;
            return "|I155|" +
                   COD_CTA.FormataParaSPED() +
                   COD_CCUS.FormataParaSPED() +
                   VL_SLD_INI.FormataParaSPED() +
                   IND_DC_INI.FormataParaSPED() +
                   VL_DEB.FormataParaSPED() +
                   VL_CRED.FormataParaSPED() +
                   VL_SLD_FIN.FormataParaSPED() +
                   IND_DC_FIN.FimDaLinha();
        }
    }

    [TxtRegisterName("I200")]
    public class SpedRegistroI200 : LeiauteSped
    {
        public string NUM_LCTO { get; set; }
        public DateTime DT_LCTO { get; set; }
        public Double VL_LCTO { get; set; }
        public string IND_LCTO { get; set; }

        public List<SpedRegistroI250> RegI250 { get; set; }

        public SpedRegistroI200()
        {
            RegI250 = new List<SpedRegistroI250>();
        }

        public override string ToString()
        {
            string Resultado = "|I200|" +
                                NUM_LCTO.FormataParaSPED() +
                                DT_LCTO.FormataParaSPED() +
                                VL_LCTO.FormataParaSPED() +
                                IND_LCTO.FormataParaSPED().FimDaLinha();
            QdeDeLinhas = 1;
            foreach (var r in RegI250)
            {
                Resultado += r.ToString();
                QdeDeLinhas += r.QdeDeLinhas;
            }
            return Resultado;
        }
    }

    [TxtRegisterName("I250")]
    public class SpedRegistroI250 : LeiauteSped
    {
        public string COD_CTA { get; set; }
        public string COD_CCUS { get; set; }
        public Double VL_DC { get; set; }
        public string IND_DC { get; set; }
        public string NUM_ARQ { get; set; }
        public string COD_HIST_PAD { get; set; }
        public override string ToString()
        {
            QdeDeLinhas = 1;
            return "|I250|" +
                   COD_CTA.FormataParaSPED() +
                   COD_CCUS.FormataParaSPED() +
                   VL_DC.FormataParaSPED() +
                   IND_DC.FormataParaSPED() +
                   NUM_ARQ.FormataParaSPED() +
                   COD_HIST_PAD.FormataParaSPED().FimDaLinha();
        }
    }

    [TxtRegisterName("I350")]
    public class SpedRegistroI350 : LeiauteSped
    {
        public DateTime DT_RES { get; set; }

        public List<SpedRegistroI355> RegI355 { get; set; }

        public SpedRegistroI350()
        {
            RegI355 = new List<SpedRegistroI355>();
        }

        public override string ToString()
        {
            string Resultado = "|I350|" +
                                DT_RES.FormataParaSPED().FimDaLinha();
            QdeDeLinhas = 1;
            foreach (var r in RegI355)
            {
                Resultado += r.ToString();
                QdeDeLinhas += r.QdeDeLinhas;
            }

            return Resultado;
        }
    }

    [TxtRegisterName("I355")]
    public class SpedRegistroI355 : LeiauteSped
    {
        public string COD_CTA { get; set; }
        public string COD_CCUS { get; set; }
        public double VL_CTA { get; set; }
        public string IND_DC { get; set; }
        public override string ToString()
        {
            QdeDeLinhas = 1;
            return "|I355|" +
                   COD_CTA.FormataParaSPED() +
                   COD_CCUS.FormataParaSPED() +
                   VL_CTA.FormataParaSPED() +
                   IND_DC.FormataParaSPED().FimDaLinha();
        }
    }

    [TxtRegisterName("I990")]
    public class SpedRegistroI990 : LeiauteSped
    {
        public override string ToString()
        {
            QdeDeLinhas++;
            return "|I990|" + QdeDeLinhas.FormataParaSPED().FimDaLinha();
        }
    }

    [TxtRegisterName("J001")]
    public class SpedRegistroJ001 : LeiauteSped
    {
        public override string ToString()
        {
            return "|J001|0".FormataParaSPED().FimDaLinha();
        }
    }

    [TxtRegisterName("J900")]
    public class SpedRegistroJ900 : LeiauteSped
    {
        public override string ToString()
        {
            QdeDeLinhas++;
            return "|J900|" + QdeDeLinhas.FormataParaSPED().FimDaLinha();
        }
    }

    [TxtRegisterName("J930")]
    public class SpedRegistroJ930 : LeiauteSped
    {
        public override string ToString()
        {
            QdeDeLinhas++;
            return "|J930|" + QdeDeLinhas.FormataParaSPED().FimDaLinha();
        }
    }

    [TxtRegisterName("J990")]
    public class SpedRegistroJ990 : LeiauteSped
    {
        public override string ToString()
        {
            QdeDeLinhas++;
            return "|J990|" + QdeDeLinhas.FormataParaSPED().FimDaLinha();
        }
    }

    [TxtRegisterName("9001")]
    public class SpedRegistro9001 : LeiauteSped
    {
        public override string ToString()
        {
            return "|9001|0".FormataParaSPED().FimDaLinha();
        }
    }

    [TxtRegisterName("9900")]
    public class SpedRegistro9900 : LeiauteSped
    {
        public override string ToString()
        {
            QdeDeLinhas++;
            return "|9900|" + QdeDeLinhas.FormataParaSPED().FimDaLinha();
        }
    }

    [TxtRegisterName("9990")]
    public class SpedRegistro9990 : LeiauteSped
    {
        public override string ToString()
        {
            QdeDeLinhas++;
            return "|9990|" + QdeDeLinhas.FormataParaSPED().FimDaLinha();
        }
    }

    [TxtRegisterName("9999")]
    public class SpedRegistro9999 : LeiauteSped
    {
        public override string ToString()
        {
            QdeDeLinhas++;
            return "|9999|" + QdeDeLinhas.FormataParaSPED().FimDaLinha();
        }
    }

    public abstract class BlocoSped
    {
        public abstract string Gerar();
    }

    public class Bloco0 : BlocoSped
    {
        public SpedRegistro0001 Reg0001 { get; set; }
        public SpedRegistro0007 Reg0007 { get; set; }
        public SpedRegistro0020 Reg0020 { get; set; }
        public SpedRegistro0990 Reg0990 { get; set; }

        public Bloco0()
        {
            Reg0001 = new SpedRegistro0001();
            Reg0007 = new SpedRegistro0007();
            Reg0020 = new SpedRegistro0020();
            Reg0990 = new SpedRegistro0990 { QdeDeLinhas = 4 };
        }

        public override string Gerar()
        {
            return Reg0001.ToString() +
                   Reg0007.ToString() +
                   Reg0020.ToString() +
                   Reg0990.ToString();
        }
    }

    public class BlocoI : BlocoSped
    {
        public SpedRegistroI001 RegI001 { get; set; }
        public SpedRegistroI990 RegI990 { get; set; }

        public BlocoI()
        {
            RegI001 = new SpedRegistroI001();
            RegI990 = new SpedRegistroI990();
        }

        public override string Gerar()
        {
            string Retorno = RegI001.ToString();
            RegI990.QdeDeLinhas = RegI001.QdeDeLinhas;
            return Retorno + RegI990.ToString();
        }
    }

    public class BlocoJ : BlocoSped
    {
        public SpedRegistroJ001 RegJ001 { get; set; }
        public SpedRegistroJ900 RegJ900 { get; set; }
        public SpedRegistroJ930 RegJ930 { get; set; }
        public SpedRegistroJ990 RegJ990 { get; set; }

        public BlocoJ()
        {
            RegJ001 = new SpedRegistroJ001();
            RegJ900 = new SpedRegistroJ900();
            RegJ930 = new SpedRegistroJ930();
            RegJ990 = new SpedRegistroJ990();
        }

        public override string Gerar()
        {
            return "";
        }
    }

    public class Bloco9 : BlocoSped
    {
        public SpedRegistro9001 Reg9001 { get; set; }
        public SpedRegistro9900 Reg9900 { get; set; }
        public SpedRegistro9990 Reg9990 { get; set; }
        public SpedRegistro9999 Reg9999 { get; set; }

        public Bloco9()
        {
            Reg9001 = new SpedRegistro9001();
            Reg9900 = new SpedRegistro9900();
            Reg9990 = new SpedRegistro9990();
            Reg9999 = new SpedRegistro9999();
        }

        public override string Gerar()
        {
            return "";
        }
    }
}
