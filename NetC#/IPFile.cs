public class IPFile{
	
	private string GenerateResultIP =  "192.168.0.2";
	private string ControlIP = "192.168.0.3";
	private string FusionIP = "192.168.0.4";

	
	private string Radar1IP = "192.168.0.5";
	private string Radar2IP = "192.168.0.6";
	private string Radar3IP = "192.168.0.7";
	private string Radar4IP = "192.168.0.8";
	private string Radar5IP = "192.168.0.9";
	
	//public string[] RadarTotalIPs = {Radar1IP,Radar2IP,Radar3IP,Radar4IP,Radar5IP};
	
	public string getGenerateResultIP(){
		return GenerateResultIP;
	}
	
	public string getControlIP(){
		return ControlIP;
	}
	
	public string getFusionIP(){
		return FusionIP;
	}
	
	public string getRadar1IP(){
		return Radar1IP;
	}
	
	public string getRadar2IP(){
		return Radar2IP;
	}
	
	public string getRadar3IP(){
		return Radar3IP;
	}
	
	public string getRadar4IP(){
		return Radar4IP;
	}
	
	public string getRadar5IP(){
		return Radar5IP;
	}
}

