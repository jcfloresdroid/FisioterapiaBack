namespace Core.Domain.Resource;

public static class ImagenesMapa
{
    private static string rutaBase = Directory.GetCurrentDirectory();
    // AREAS ANTERIOR
    private static string Cabeza = Path.Combine(rutaBase, "wwwroot", "MapaCorporal", "AreasAnterior", "Cabeza.png");
    private static string RCervicalA = Path.Combine(rutaBase, "wwwroot", "MapaCorporal", "AreasAnterior", "RegionCervicalAnterior.png"); 
    private static string DeltoidesAI = Path.Combine(rutaBase, "wwwroot", "MapaCorporal", "AreasAnterior", "DeltoidesAnteriorIzquierdo.png");
    private static string DeltoidesAD = Path.Combine(rutaBase, "wwwroot", "MapaCorporal", "AreasAnterior", "DeltoidesAnteriorDerecho.png");
    private static string BicepI = Path.Combine(rutaBase, "wwwroot", "MapaCorporal", "AreasAnterior", "BicepIzquierdo.png");
    private static string BicepD = Path.Combine(rutaBase, "wwwroot", "MapaCorporal", "AreasAnterior", "BicepDerecho.png");
    private static string AntebrazoAI = Path.Combine(rutaBase, "wwwroot", "MapaCorporal", "AreasAnterior", "AntebrazoAnteriorIzquierdo.png");
    private static string AntebrazoAD = Path.Combine(rutaBase, "wwwroot", "MapaCorporal", "AreasAnterior", "AntebrazoAnteriorDerecho.png");
    private static string MuñecaI = Path.Combine(rutaBase, "wwwroot", "MapaCorporal", "AreasAnterior", "MuñecaIzquierda.png");
    private static string MuñecaD = Path.Combine(rutaBase, "wwwroot", "MapaCorporal", "AreasAnterior", "MuñecaDerecha.png");
    private static string ManoI = Path.Combine(rutaBase, "wwwroot", "MapaCorporal", "AreasAnterior", "ManoIzquierda.png");
    private static string ManoD = Path.Combine(rutaBase, "wwwroot", "MapaCorporal", "AreasAnterior", "ManoDerecha.png");
    private static string PechoI = Path.Combine(rutaBase, "wwwroot", "MapaCorporal", "AreasAnterior", "PechoIzquierdo.png");
    private static string PechoD = Path.Combine(rutaBase, "wwwroot", "MapaCorporal", "AreasAnterior", "PechoDerecho.png");
    private static string RAbdominal = Path.Combine(rutaBase, "wwwroot", "MapaCorporal", "AreasAnterior", "RegionAbdominal.png");
    private static string Pelvis = Path.Combine(rutaBase, "wwwroot", "MapaCorporal", "AreasAnterior", "Pelvis.png");
    private static string MusloAI = Path.Combine(rutaBase, "wwwroot", "MapaCorporal", "AreasAnterior", "MusloAnteriorIzquierdo.png");
    private static string MusloAD = Path.Combine(rutaBase, "wwwroot", "MapaCorporal", "AreasAnterior", "MusloAnteriorDerecho.png");
    private static string RodillaAI = Path.Combine(rutaBase, "wwwroot", "MapaCorporal", "AreasAnterior", "RodillaAnteriorIzquierda.png");
    private static string RodillaAD = Path.Combine(rutaBase, "wwwroot", "MapaCorporal", "AreasAnterior", "RodillaAnteriorDerecha.png");
    private static string TibialD = Path.Combine(rutaBase, "wwwroot", "MapaCorporal", "AreasAnterior", "TibialDerecho.png");
    private static string TibialI = Path.Combine(rutaBase, "wwwroot", "MapaCorporal", "AreasAnterior", "TibialIzquierdo.png");
    private static string TobilloI = Path.Combine(rutaBase, "wwwroot", "MapaCorporal", "AreasAnterior", "TobilloIzquierdo.png");
    private static string TobilloD = Path.Combine(rutaBase, "wwwroot", "MapaCorporal", "AreasAnterior", "TobilloDerecho.png");
    private static string EmpeineI = Path.Combine(rutaBase, "wwwroot", "MapaCorporal", "AreasAnterior", "EmpeineIzquierdo.png");
    private static string EmpeineD = Path.Combine(rutaBase, "wwwroot", "MapaCorporal", "AreasAnterior", "EmpeineDerecho.png");

    // AREAS POSTERIOR
    private static string Occipital = Path.Combine(rutaBase, "wwwroot", "MapaCorporal", "AreasPosterior", "Occipital.png");
    private static string CervicalP = Path.Combine(rutaBase, "wwwroot", "MapaCorporal", "AreeasPosterior", "CervicalPosterior.png");
    private static string RToracica = Path.Combine(rutaBase, "wwwroot", "MapaCorporal", "AreasPosterior", "RegionToracica.png");
    private static string RLumbar = Path.Combine(rutaBase, "wwwroot", "MapaCorporal", "AreasPosterior", "RegionLumbar.png");
    private static string REscapularIzquierda = Path.Combine(rutaBase, "wwwroot", "MapaCorporal", "AreasPosterior", "RegionEscapularIzquierda.png");
    private static string REscapularD = Path.Combine(rutaBase, "wwwroot", "MapaCorporal", "AreasPosterior", "RegionEscapularDerecha.png");
    private static string TricepsI = Path.Combine(rutaBase, "wwwroot", "MapaCorporal", "AreasPosterior", "TricepIzquierdo.png");
    private static string TricepsD = Path.Combine(rutaBase, "wwwroot", "MapaCorporal", "AreasPosterior", "TricepDerecho.png");
    private static string AntebrazoPI = Path.Combine(rutaBase, "wwwroot", "MapaCorporal", "AreasPosterior", "AntebrazoPosteriorIzquierdo.png");
    private static string AntebrazoPD = Path.Combine(rutaBase, "wwwroot", "MapaCorporal", "AreasPosterior", "AntebrazoPosteriorDerecho.png");
    private static string GluteoI = Path.Combine(rutaBase, "wwwroot", "MapaCorporal", "AreasPosterior", "GluteoIzquierdo.png");
    private static string GluteoD = Path.Combine(rutaBase, "wwwroot", "MapaCorporal", "AreasPosterior", "GluteoDerecho.png");
    private static string IsquiotibialesI = Path.Combine(rutaBase, "wwwroot", "MapaCorporal", "AreasPosterior", "IsquiotibialIzquierdo.png");
    private static string IsquiotibialesD = Path.Combine(rutaBase, "wwwroot", "MapaCorporal", "AreasPosterior", "IsquiotibialDerecho.png");
    private static string RPopliteaI = Path.Combine(rutaBase, "wwwroot", "MapaCorporal", "AreasPosterior", "RegionPoplipteaIzquierda.png");
    private static string RPopliteaD = Path.Combine(rutaBase, "wwwroot", "MapaCorporal", "AreasPosterior", "RegionPoplipteaDerecha.png");
    private static string PantorrillaI = Path.Combine(rutaBase, "wwwroot", "MapaCorporal", "AreasPosterior", "PantorrillaIzquierda.png");
    private static string PantorrillaD = Path.Combine(rutaBase, "wwwroot", "MapaCorporal", "AreasPosterior", "PantorrillaDerecha.png");
    private static string TendonAquilesI = Path.Combine(rutaBase, "wwwroot", "MapaCorporal", "AreasPosterior", "TendonAquilesIzquierdo.png");
    private static string TendonAquilesD = Path.Combine(rutaBase, "wwwroot", "MapaCorporal", "AreasPosterior", "TendonAquilesDerecho.png");
    private static string TalonI = Path.Combine(rutaBase, "wwwroot", "MapaCorporal", "AreasPosterior", "TalonIzquierdo.png");
    private static string TalonD = Path.Combine(rutaBase, "wwwroot", "MapaCorporal", "AreasPosterior", "TalonDerecho.png");

    // AREAS LATERAL
    private static string RTemporal = Path.Combine(rutaBase, "wwwroot", "MapaCorporal", "AreasLateral", "RegionTemporal.png");
    private static string RMandibular = Path.Combine(rutaBase, "wwwroot", "MapaCorporal", "AreasLateral", "RegionMandibular.png");
    private static string RCervicalL	 = Path.Combine(rutaBase, "wwwroot", "MapaCorporal", "AreasLateral", "RegionCervicalLateral.png");
    private static string RtoracicaL = Path.Combine(rutaBase, "wwwroot", "MapaCorporal", "AreasLateral", "RegionToracicaLateral.png");
    private static string DeltoidesL = Path.Combine(rutaBase, "wwwroot", "MapaCorporal", "AreasLateral", "DeltoidesLateral.png");
    private static string Codo = Path.Combine(rutaBase, "wwwroot", "MapaCorporal", "AreasLateral", "Codo.png");
    private static string RIliaca = Path.Combine(rutaBase, "wwwroot", "MapaCorporal", "AreasLateral", "RegionIliaca.png");
    private static string MusloL = Path.Combine(rutaBase, "wwwroot", "MapaCorporal", "AreasLateral", "GluteoLateral.png");

    public static Dictionary<int, string> MapaHombre()
    {
        Dictionary<int, string> image = new Dictionary<int, string>
        {
            { 1, Cabeza },
            { 2, RCervicalA},
            { 3, DeltoidesAI},
            { 4, DeltoidesAD},
            { 5, BicepI},
            { 6, BicepD},
            { 7, AntebrazoAI},
            { 8, AntebrazoAD},
            { 9, MuñecaI},
            { 10, MuñecaD},
            { 11, ManoI},
            { 12, ManoD},
            { 13, PechoI},
            { 14, PechoD},
            { 15, RAbdominal},
            { 16, Pelvis},
            { 17, MusloAI},
            { 18, MusloAD},
            { 19, RodillaAI},
            { 20, RodillaAD},
            { 21, TibialD},
            { 22, TibialI},
            { 23, TobilloD},
            { 24, TobilloI},
            { 25, EmpeineI},
            { 26, EmpeineD},
            { 27, Occipital},
            { 28, CervicalP},
            { 29, RToracica},
            { 30, RLumbar},
            { 31, REscapularIzquierda},
            { 32, REscapularD},
            { 33, TricepsI},
            { 34, TricepsD},
            { 35, AntebrazoPI},
            { 36, AntebrazoPD},
            { 37, GluteoI},
            { 38, GluteoD},
            { 39, IsquiotibialesI},
            { 40, IsquiotibialesD},
            { 41, RPopliteaI},
            { 42, RPopliteaD},
            { 43, PantorrillaI},
            { 44, PantorrillaD},
            { 45, TendonAquilesI},
            { 46, TendonAquilesD},
            { 47, TalonI},
            { 48, TalonD},
            { 49, RTemporal},
            { 50, RMandibular},
            { 51, RCervicalL},
            { 52, RtoracicaL},
            { 53, DeltoidesL},
            { 54, Codo},
            { 55, RIliaca},
            { 56, MusloL}
        };
        
        return image;
    }
}