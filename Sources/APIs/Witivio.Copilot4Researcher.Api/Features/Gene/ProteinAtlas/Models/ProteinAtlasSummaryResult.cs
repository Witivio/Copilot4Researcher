using System.Text.Json.Serialization;

namespace Witivio.Copilot4Researcher.Features.Gene.ProteinAtlas.Models
{

    public class ProteinAtlasSummaryResult
    {
        [JsonPropertyName("Gene")]
        public string Gene { get; set; }

        [JsonPropertyName("Gene synonym")]
        public List<string> GeneSynonym { get; set; }

        [JsonPropertyName("Ensembl")]
        public string Ensembl { get; set; }

        [JsonPropertyName("Gene description")]
        public string GeneDescription { get; set; }

        [JsonPropertyName("Uniprot")]
        public List<string> Uniprot { get; set; }

        [JsonPropertyName("Chromosome")]
        public string Chromosome { get; set; }

        [JsonPropertyName("Position")]
        public string Position { get; set; }

        [JsonPropertyName("Protein class")]
        public List<string> ProteinClass { get; set; }

        [JsonPropertyName("Biological process")]
        public List<string> BiologicalProcess { get; set; }

        [JsonPropertyName("Molecular function")]
        public List<string> MolecularFunction { get; set; }

        [JsonPropertyName("Disease involvement")]
        public List<string> DiseaseInvolvement { get; set; }

        [JsonPropertyName("Evidence")]
        public string Evidence { get; set; }

        [JsonPropertyName("HPA evidence")]
        public string HPAEvidence { get; set; }

        [JsonPropertyName("UniProt evidence")]
        public string UniProtEvidence { get; set; }

        [JsonPropertyName("NeXtProt evidence")]
        public string NeXtProtEvidence { get; set; }

        [JsonPropertyName("RNA tissue specificity")]
        public string RNATissueSpecificity { get; set; }



        [JsonPropertyName("Antibody")]
        public List<string> Antibody { get; set; }

        [JsonPropertyName("Reliability (IH)")]
        public string ReliabilityIH { get; set; }

        [JsonPropertyName("Reliability (Mouse Brain)")]
        public object ReliabilityMouseBrain { get; set; }

        [JsonPropertyName("Reliability (IF)")]
        public string ReliabilityIF { get; set; }

        [JsonPropertyName("Subcellular location")]
        public List<string> SubcellularLocation { get; set; }

        [JsonPropertyName("Secretome location")]
        public object SecretomeLocation { get; set; }

        [JsonPropertyName("Secretome function")]
        public object SecretomeFunction { get; set; }

        [JsonPropertyName("CCD Protein")]
        public string CCDProtein { get; set; }

        [JsonPropertyName("CCD Transcript")]
        public string CCDTranscript { get; set; }

        [JsonPropertyName("Blood concentration - Conc. blood IM [pg/L]")]
        public object BloodConcentrationConcBloodIMPgL { get; set; }

        [JsonPropertyName("Blood concentration - Conc. blood MS [pg/L]")]
        public object BloodConcentrationConcBloodMSPgL { get; set; }

        [JsonPropertyName("Blood expression cluster")]
        public string BloodExpressionCluster { get; set; }

        [JsonPropertyName("Tissue expression cluster")]
        public string TissueExpressionCluster { get; set; }

        [JsonPropertyName("Brain expression cluster")]
        public string BrainExpressionCluster { get; set; }

        [JsonPropertyName("Cell line expression cluster")]
        public string CellLineExpressionCluster { get; set; }

        [JsonPropertyName("Single cell expression cluster")]
        public string SingleCellExpressionCluster { get; set; }

        [JsonPropertyName("Interactions")]
        public int Interactions { get; set; }

        [JsonPropertyName("Subcellular main location")]
        public List<string> SubcellularMainLocation { get; set; }

        [JsonPropertyName("Subcellular additional location")]
        public object SubcellularAdditionalLocation { get; set; }

        [JsonPropertyName("Antibody RRID")]
        public AntibodyRRID AntibodyRRID { get; set; }

    }


}
