// Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);
using System.Text.Json.Serialization;

public class Achievement
    {
        [JsonPropertyName("groupId")]
        public string GroupId { get; set; }

        [JsonPropertyName("numSubjects")]
        public string NumSubjects { get; set; }
    }

    public class AdverseEventsModule
    {
        [JsonPropertyName("frequencyThreshold")]
        public string FrequencyThreshold { get; set; }

        [JsonPropertyName("timeFrame")]
        public string TimeFrame { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("eventGroups")]
        public List<EventGroup> EventGroups { get; set; }

        [JsonPropertyName("seriousEvents")]
        public List<SeriousEvent> SeriousEvents { get; set; }

        [JsonPropertyName("otherEvents")]
        public List<OtherEvent> OtherEvents { get; set; }
    }

    public class Ancestor
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("term")]
        public string Term { get; set; }
    }

    public class ArmGroup
    {
        [JsonPropertyName("label")]
        public string Label { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("interventionNames")]
        public List<string> InterventionNames { get; set; }
    }

    public class ArmsInterventionsModule
    {
        [JsonPropertyName("armGroups")]
        public List<ArmGroup> ArmGroups { get; set; }

        [JsonPropertyName("interventions")]
        public List<Intervention> Interventions { get; set; }
    }

    public class BaselineCharacteristicsModule
    {
        [JsonPropertyName("groups")]
        public List<Group> Groups { get; set; }

        [JsonPropertyName("denoms")]
        public List<Denom> Denoms { get; set; }

        [JsonPropertyName("measures")]
        public List<Measure> Measures { get; set; }

        [JsonPropertyName("populationDescription")]
        public string PopulationDescription { get; set; }
    }

    public class BrowseBranch
    {
        [JsonPropertyName("abbrev")]
        public string Abbrev { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class BrowseLeaf
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("asFound")]
        public string AsFound { get; set; }

        [JsonPropertyName("relevance")]
        public string Relevance { get; set; }
    }

    public class Category
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("measurements")]
        public List<Measurement> Measurements { get; set; }
    }

    public class CentralContact
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("role")]
        public string Role { get; set; }

        [JsonPropertyName("phone")]
        public string Phone { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }
    }

    public class CertainAgreement
    {
        [JsonPropertyName("piSponsorEmployee")]
        public bool PiSponsorEmployee { get; set; }

        [JsonPropertyName("restrictiveAgreement")]
        public bool RestrictiveAgreement { get; set; }

        [JsonPropertyName("restrictionType")]
        public string RestrictionType { get; set; }

        [JsonPropertyName("otherDetails")]
        public string OtherDetails { get; set; }
    }

    public class Class
    {
        [JsonPropertyName("categories")]
        public List<Category> Categories { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("denoms")]
        public List<Denom> Denoms { get; set; }
    }

    public class Collaborator
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("class")]
        public string Class { get; set; }
    }

    public class CompletionDateStruct
    {
        [JsonPropertyName("date")]
        public string Date { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }

    public class ConditionBrowseModule
    {
        [JsonPropertyName("meshes")]
        public List<Mesh> Meshes { get; set; }

        [JsonPropertyName("ancestors")]
        public List<Ancestor> Ancestors { get; set; }

        [JsonPropertyName("browseLeaves")]
        public List<BrowseLeaf> BrowseLeaves { get; set; }

        [JsonPropertyName("browseBranches")]
        public List<BrowseBranch> BrowseBranches { get; set; }
    }

    public class ConditionsModule
    {
        [JsonPropertyName("conditions")]
        public List<string> Conditions { get; set; }

        [JsonPropertyName("keywords")]
        public List<string> Keywords { get; set; }
    }

    public class Contact
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("role")]
        public string Role { get; set; }

        [JsonPropertyName("phone")]
        public string Phone { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }
    }

    public class ContactsLocationsModule
    {
        [JsonPropertyName("overallOfficials")]
        public List<OverallOfficial> OverallOfficials { get; set; }

        [JsonPropertyName("locations")]
        public List<Location> Locations { get; set; }

        [JsonPropertyName("centralContacts")]
        public List<CentralContact> CentralContacts { get; set; }
    }

    public class Count
    {
        [JsonPropertyName("groupId")]
        public string GroupId { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }
    }

    public class Denom
    {
        [JsonPropertyName("units")]
        public string Units { get; set; }

        [JsonPropertyName("counts")]
        public List<Count> Counts { get; set; }
    }

    public class DerivedSection
    {
        [JsonPropertyName("miscInfoModule")]
        public MiscInfoModule MiscInfoModule { get; set; }

        [JsonPropertyName("conditionBrowseModule")]
        public ConditionBrowseModule ConditionBrowseModule { get; set; }

        [JsonPropertyName("interventionBrowseModule")]
        public InterventionBrowseModule InterventionBrowseModule { get; set; }
    }

    public class DescriptionModule
    {
        [JsonPropertyName("briefSummary")]
        public string BriefSummary { get; set; }

        [JsonPropertyName("detailedDescription")]
        public string DetailedDescription { get; set; }
    }

    public class DesignInfo
    {
        [JsonPropertyName("allocation")]
        public string Allocation { get; set; }

        [JsonPropertyName("interventionModel")]
        public string InterventionModel { get; set; }

        [JsonPropertyName("primaryPurpose")]
        public string PrimaryPurpose { get; set; }

        [JsonPropertyName("maskingInfo")]
        public MaskingInfo MaskingInfo { get; set; }

        [JsonPropertyName("observationalModel")]
        public string ObservationalModel { get; set; }

        [JsonPropertyName("timePerspective")]
        public string TimePerspective { get; set; }

        [JsonPropertyName("interventionModelDescription")]
        public string InterventionModelDescription { get; set; }
    }

    public class DesignModule
    {
        [JsonPropertyName("studyType")]
        public string StudyType { get; set; }

        [JsonPropertyName("phases")]
        public List<string> Phases { get; set; }

        [JsonPropertyName("designInfo")]
        public DesignInfo DesignInfo { get; set; }

        [JsonPropertyName("enrollmentInfo")]
        public EnrollmentInfo EnrollmentInfo { get; set; }
    }

    public class DocumentSection
    {
        [JsonPropertyName("largeDocumentModule")]
        public LargeDocumentModule LargeDocumentModule { get; set; }
    }

    public class DropWithdraw
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("reasons")]
        public List<Reason> Reasons { get; set; }
    }

    public class EligibilityModule
    {
        [JsonPropertyName("eligibilityCriteria")]
        public string EligibilityCriteria { get; set; }

        [JsonPropertyName("healthyVolunteers")]
        public bool HealthyVolunteers { get; set; }

        [JsonPropertyName("sex")]
        public string Sex { get; set; }

        [JsonPropertyName("minimumAge")]
        public string MinimumAge { get; set; }

        [JsonPropertyName("stdAges")]
        public List<string> StdAges { get; set; }

        [JsonPropertyName("maximumAge")]
        public string MaximumAge { get; set; }

        [JsonPropertyName("studyPopulation")]
        public string StudyPopulation { get; set; }

        [JsonPropertyName("samplingMethod")]
        public string SamplingMethod { get; set; }
    }

    public class EnrollmentInfo
    {
        [JsonPropertyName("count")]
        public int Count { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }

    public class EventGroup
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("seriousNumAffected")]
        public int SeriousNumAffected { get; set; }

        [JsonPropertyName("seriousNumAtRisk")]
        public int SeriousNumAtRisk { get; set; }

        [JsonPropertyName("otherNumAffected")]
        public int OtherNumAffected { get; set; }

        [JsonPropertyName("otherNumAtRisk")]
        public int OtherNumAtRisk { get; set; }

        [JsonPropertyName("deathsNumAffected")]
        public int? DeathsNumAffected { get; set; }

        [JsonPropertyName("deathsNumAtRisk")]
        public int? DeathsNumAtRisk { get; set; }
    }

    public class ExpandedAccessInfo
    {
        [JsonPropertyName("hasExpandedAccess")]
        public bool HasExpandedAccess { get; set; }
    }

    public class GeoPoint
    {
        [JsonPropertyName("lat")]
        public double Lat { get; set; }

        [JsonPropertyName("lon")]
        public double Lon { get; set; }
    }

    public class Group
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }

    public class IdentificationModule
    {
        [JsonPropertyName("nctId")]
        public string NctId { get; set; }

        [JsonPropertyName("orgStudyIdInfo")]
        public OrgStudyIdInfo OrgStudyIdInfo { get; set; }

        [JsonPropertyName("secondaryIdInfos")]
        public List<SecondaryIdInfo> SecondaryIdInfos { get; set; }

        [JsonPropertyName("organization")]
        public Organization Organization { get; set; }

        [JsonPropertyName("briefTitle")]
        public string BriefTitle { get; set; }

        [JsonPropertyName("officialTitle")]
        public string OfficialTitle { get; set; }

        [JsonPropertyName("acronym")]
        public string Acronym { get; set; }
    }

    public class Intervention
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("armGroupLabels")]
        public List<string> ArmGroupLabels { get; set; }

        [JsonPropertyName("otherNames")]
        public List<string> OtherNames { get; set; }
    }

    public class InterventionBrowseModule
    {
        [JsonPropertyName("meshes")]
        public List<Mesh> Meshes { get; set; }

        [JsonPropertyName("ancestors")]
        public List<Ancestor> Ancestors { get; set; }

        [JsonPropertyName("browseLeaves")]
        public List<BrowseLeaf> BrowseLeaves { get; set; }

        [JsonPropertyName("browseBranches")]
        public List<BrowseBranch> BrowseBranches { get; set; }
    }

    public class IpdSharingStatementModule
    {
        [JsonPropertyName("ipdSharing")]
        public string IpdSharing { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }
    }

    public class LargeDoc
    {
        [JsonPropertyName("typeAbbrev")]
        public string TypeAbbrev { get; set; }

        [JsonPropertyName("hasProtocol")]
        public bool HasProtocol { get; set; }

        [JsonPropertyName("hasSap")]
        public bool HasSap { get; set; }

        [JsonPropertyName("hasIcf")]
        public bool HasIcf { get; set; }

        [JsonPropertyName("label")]
        public string Label { get; set; }

        [JsonPropertyName("date")]
        public string Date { get; set; }

        [JsonPropertyName("uploadDate")]
        public string UploadDate { get; set; }

        [JsonPropertyName("filename")]
        public string Filename { get; set; }

        [JsonPropertyName("size")]
        public int Size { get; set; }
    }

    public class LargeDocumentModule
    {
        [JsonPropertyName("largeDocs")]
        public List<LargeDoc> LargeDocs { get; set; }
    }

    public class LastUpdatePostDateStruct
    {
        [JsonPropertyName("date")]
        public string Date { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }

    public class LeadSponsor
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("class")]
        public string Class { get; set; }
    }

    public class LimitationsAndCaveats
    {
        [JsonPropertyName("description")]
        public string Description { get; set; }
    }

    public class Location
    {
        [JsonPropertyName("facility")]
        public string Facility { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("state")]
        public string State { get; set; }

        [JsonPropertyName("zip")]
        public string Zip { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; }

        [JsonPropertyName("contacts")]
        public List<Contact> Contacts { get; set; }

        [JsonPropertyName("geoPoint")]
        public GeoPoint GeoPoint { get; set; }
    }

    public class MaskingInfo
    {
        [JsonPropertyName("masking")]
        public string Masking { get; set; }
    }

    public class Measure
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("paramType")]
        public string ParamType { get; set; }

        [JsonPropertyName("unitOfMeasure")]
        public string UnitOfMeasure { get; set; }

        [JsonPropertyName("classes")]
        public List<Class> Classes { get; set; }

        [JsonPropertyName("dispersionType")]
        public string DispersionType { get; set; }

        [JsonPropertyName("calculatePct")]
        public bool? CalculatePct { get; set; }
    }

    public class Measurement
    {
        [JsonPropertyName("groupId")]
        public string GroupId { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }

        [JsonPropertyName("spread")]
        public string Spread { get; set; }

        [JsonPropertyName("lowerLimit")]
        public string LowerLimit { get; set; }

        [JsonPropertyName("upperLimit")]
        public string UpperLimit { get; set; }

        [JsonPropertyName("comment")]
        public string Comment { get; set; }
    }

    public class Mesh
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("term")]
        public string Term { get; set; }
    }

    public class Milestone
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("achievements")]
        public List<Achievement> Achievements { get; set; }
    }

    public class MiscInfoModule
    {
        [JsonPropertyName("versionHolder")]
        public string VersionHolder { get; set; }
    }

    public class MoreInfoModule
    {
        [JsonPropertyName("limitationsAndCaveats")]
        public LimitationsAndCaveats LimitationsAndCaveats { get; set; }

        [JsonPropertyName("certainAgreement")]
        public CertainAgreement CertainAgreement { get; set; }

        [JsonPropertyName("pointOfContact")]
        public PointOfContact PointOfContact { get; set; }
    }

    public class Organization
    {
        [JsonPropertyName("fullName")]
        public string FullName { get; set; }

        [JsonPropertyName("class")]
        public string Class { get; set; }
    }

    public class OrgStudyIdInfo
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
    }

    public class OtherEvent
    {
        [JsonPropertyName("term")]
        public string Term { get; set; }

        [JsonPropertyName("organSystem")]
        public string OrganSystem { get; set; }

        [JsonPropertyName("sourceVocabulary")]
        public string SourceVocabulary { get; set; }

        [JsonPropertyName("assessmentType")]
        public string AssessmentType { get; set; }

        [JsonPropertyName("stats")]
        public List<Stat> Stats { get; set; }
    }

    public class OutcomeMeasure
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("populationDescription")]
        public string PopulationDescription { get; set; }

        [JsonPropertyName("reportingStatus")]
        public string ReportingStatus { get; set; }

        [JsonPropertyName("paramType")]
        public string ParamType { get; set; }

        [JsonPropertyName("dispersionType")]
        public string DispersionType { get; set; }

        [JsonPropertyName("unitOfMeasure")]
        public string UnitOfMeasure { get; set; }

        [JsonPropertyName("timeFrame")]
        public string TimeFrame { get; set; }

        [JsonPropertyName("groups")]
        public List<Group> Groups { get; set; }

        [JsonPropertyName("denoms")]
        public List<Denom> Denoms { get; set; }

        [JsonPropertyName("classes")]
        public List<Class> Classes { get; set; }
    }

    public class OutcomeMeasuresModule
    {
        [JsonPropertyName("outcomeMeasures")]
        public List<OutcomeMeasure> OutcomeMeasures { get; set; }
    }

    public class OutcomesModule
    {
        [JsonPropertyName("primaryOutcomes")]
        public List<PrimaryOutcome> PrimaryOutcomes { get; set; }

        [JsonPropertyName("secondaryOutcomes")]
        public List<SecondaryOutcome> SecondaryOutcomes { get; set; }
    }

    public class OverallOfficial
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("affiliation")]
        public string Affiliation { get; set; }

        [JsonPropertyName("role")]
        public string Role { get; set; }
    }

    public class OversightModule
    {
        [JsonPropertyName("oversightHasDmc")]
        public bool OversightHasDmc { get; set; }

        [JsonPropertyName("isFdaRegulatedDrug")]
        public bool IsFdaRegulatedDrug { get; set; }

        [JsonPropertyName("isFdaRegulatedDevice")]
        public bool IsFdaRegulatedDevice { get; set; }

        [JsonPropertyName("isUsExport")]
        public bool IsUsExport { get; set; }
    }

    public class ParticipantFlowModule
    {
        [JsonPropertyName("preAssignmentDetails")]
        public string PreAssignmentDetails { get; set; }

        [JsonPropertyName("recruitmentDetails")]
        public string RecruitmentDetails { get; set; }

        [JsonPropertyName("groups")]
        public List<Group> Groups { get; set; }

        [JsonPropertyName("periods")]
        public List<Period> Periods { get; set; }
    }

    public class Period
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("milestones")]
        public List<Milestone> Milestones { get; set; }

        [JsonPropertyName("dropWithdraws")]
        public List<DropWithdraw> DropWithdraws { get; set; }
    }

    public class PointOfContact
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("organization")]
        public string Organization { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("phone")]
        public string Phone { get; set; }
    }

    public class PrimaryCompletionDateStruct
    {
        [JsonPropertyName("date")]
        public string Date { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }

    public class PrimaryOutcome
    {
        [JsonPropertyName("measure")]
        public string Measure { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("timeFrame")]
        public string TimeFrame { get; set; }
    }

    public class ProtocolSection
    {
        [JsonPropertyName("identificationModule")]
        public IdentificationModule IdentificationModule { get; set; }

        [JsonPropertyName("statusModule")]
        public StatusModule StatusModule { get; set; }

        [JsonPropertyName("sponsorCollaboratorsModule")]
        public SponsorCollaboratorsModule SponsorCollaboratorsModule { get; set; }

        [JsonPropertyName("oversightModule")]
        public OversightModule OversightModule { get; set; }

        [JsonPropertyName("descriptionModule")]
        public DescriptionModule DescriptionModule { get; set; }

        [JsonPropertyName("conditionsModule")]
        public ConditionsModule ConditionsModule { get; set; }

        [JsonPropertyName("designModule")]
        public DesignModule DesignModule { get; set; }

        [JsonPropertyName("armsInterventionsModule")]
        public ArmsInterventionsModule ArmsInterventionsModule { get; set; }

        [JsonPropertyName("outcomesModule")]
        public OutcomesModule OutcomesModule { get; set; }

        [JsonPropertyName("eligibilityModule")]
        public EligibilityModule EligibilityModule { get; set; }

        [JsonPropertyName("contactsLocationsModule")]
        public ContactsLocationsModule ContactsLocationsModule { get; set; }

        [JsonPropertyName("ipdSharingStatementModule")]
        public IpdSharingStatementModule IpdSharingStatementModule { get; set; }

        [JsonPropertyName("referencesModule")]
        public ReferencesModule ReferencesModule { get; set; }
    }

    public class Reason
    {
        [JsonPropertyName("groupId")]
        public string GroupId { get; set; }

        [JsonPropertyName("numSubjects")]
        public string NumSubjects { get; set; }
    }

    public class Reference
    {
        [JsonPropertyName("pmid")]
        public string Pmid { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("citation")]
        public string Citation { get; set; }
    }

    public class ReferencesModule
    {
        [JsonPropertyName("references")]
        public List<Reference> References { get; set; }
    }

    public class ResponsibleParty
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("investigatorFullName")]
        public string InvestigatorFullName { get; set; }

        [JsonPropertyName("investigatorTitle")]
        public string InvestigatorTitle { get; set; }

        [JsonPropertyName("investigatorAffiliation")]
        public string InvestigatorAffiliation { get; set; }
    }

    public class ResultsFirstPostDateStruct
    {
        [JsonPropertyName("date")]
        public string Date { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }

    public class ResultsSection
    {
        [JsonPropertyName("participantFlowModule")]
        public ParticipantFlowModule ParticipantFlowModule { get; set; }

        [JsonPropertyName("baselineCharacteristicsModule")]
        public BaselineCharacteristicsModule BaselineCharacteristicsModule { get; set; }

        [JsonPropertyName("outcomeMeasuresModule")]
        public OutcomeMeasuresModule OutcomeMeasuresModule { get; set; }

        [JsonPropertyName("adverseEventsModule")]
        public AdverseEventsModule AdverseEventsModule { get; set; }

        [JsonPropertyName("moreInfoModule")]
        public MoreInfoModule MoreInfoModule { get; set; }
    }

    public class ClinicalTrialSearchResult
    {
        [JsonPropertyName("studies")]
        public List<Study> Studies { get; set; }

        [JsonPropertyName("nextPageToken")]
        public string NextPageToken { get; set; }
    }

    public class SecondaryIdInfo
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("domain")]
        public string Domain { get; set; }

        [JsonPropertyName("link")]
        public string Link { get; set; }
    }

    public class SecondaryOutcome
    {
        [JsonPropertyName("measure")]
        public string Measure { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("timeFrame")]
        public string TimeFrame { get; set; }
    }

    public class SeriousEvent
    {
        [JsonPropertyName("term")]
        public string Term { get; set; }

        [JsonPropertyName("organSystem")]
        public string OrganSystem { get; set; }

        [JsonPropertyName("sourceVocabulary")]
        public string SourceVocabulary { get; set; }

        [JsonPropertyName("assessmentType")]
        public string AssessmentType { get; set; }

        [JsonPropertyName("stats")]
        public List<Stat> Stats { get; set; }
    }

    public class SponsorCollaboratorsModule
    {
        [JsonPropertyName("responsibleParty")]
        public ResponsibleParty ResponsibleParty { get; set; }

        [JsonPropertyName("leadSponsor")]
        public LeadSponsor LeadSponsor { get; set; }

        [JsonPropertyName("collaborators")]
        public List<Collaborator> Collaborators { get; set; }
    }

    public class StartDateStruct
    {
        [JsonPropertyName("date")]
        public string Date { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }

    public class Stat
    {
        [JsonPropertyName("groupId")]
        public string GroupId { get; set; }

        [JsonPropertyName("numAffected")]
        public int NumAffected { get; set; }

        [JsonPropertyName("numAtRisk")]
        public int NumAtRisk { get; set; }

        [JsonPropertyName("numEvents")]
        public int? NumEvents { get; set; }
    }

    public class StatusModule
    {
        [JsonPropertyName("statusVerifiedDate")]
        public string StatusVerifiedDate { get; set; }

        [JsonPropertyName("overallStatus")]
        public string OverallStatus { get; set; }

        [JsonPropertyName("startDateStruct")]
        public StartDateStruct StartDateStruct { get; set; }

        [JsonPropertyName("primaryCompletionDateStruct")]
        public PrimaryCompletionDateStruct PrimaryCompletionDateStruct { get; set; }

        [JsonPropertyName("completionDateStruct")]
        public CompletionDateStruct CompletionDateStruct { get; set; }

        [JsonPropertyName("studyFirstSubmitDate")]
        public string StudyFirstSubmitDate { get; set; }

        [JsonPropertyName("studyFirstSubmitQcDate")]
        public string StudyFirstSubmitQcDate { get; set; }

        [JsonPropertyName("studyFirstPostDateStruct")]
        public StudyFirstPostDateStruct StudyFirstPostDateStruct { get; set; }

        [JsonPropertyName("lastUpdateSubmitDate")]
        public string LastUpdateSubmitDate { get; set; }

        [JsonPropertyName("lastUpdatePostDateStruct")]
        public LastUpdatePostDateStruct LastUpdatePostDateStruct { get; set; }

        [JsonPropertyName("expandedAccessInfo")]
        public ExpandedAccessInfo ExpandedAccessInfo { get; set; }

        [JsonPropertyName("resultsFirstSubmitDate")]
        public string ResultsFirstSubmitDate { get; set; }

        [JsonPropertyName("resultsFirstSubmitQcDate")]
        public string ResultsFirstSubmitQcDate { get; set; }

        [JsonPropertyName("resultsFirstPostDateStruct")]
        public ResultsFirstPostDateStruct ResultsFirstPostDateStruct { get; set; }

        [JsonPropertyName("whyStopped")]
        public string WhyStopped { get; set; }

        [JsonPropertyName("lastKnownStatus")]
        public string LastKnownStatus { get; set; }
    }

    public class Study
    {
        [JsonPropertyName("protocolSection")]
        public ProtocolSection ProtocolSection { get; set; }

        [JsonPropertyName("derivedSection")]
        public DerivedSection DerivedSection { get; set; }

        [JsonPropertyName("hasResults")]
        public bool HasResults { get; set; }

        [JsonPropertyName("resultsSection")]
        public ResultsSection ResultsSection { get; set; }

        [JsonPropertyName("documentSection")]
        public DocumentSection DocumentSection { get; set; }
    }

    public class StudyFirstPostDateStruct
    {
        [JsonPropertyName("date")]
        public string Date { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }

