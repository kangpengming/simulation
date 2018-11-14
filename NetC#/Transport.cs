public class Transport{
	
	
	private int[] Radar1Results = new int[2]{4010,4011};
	private int[] Radar2Results = new int[2]{4012,4011};
	private int[] Radar3Results = new int[2]{4014,4011};
	private int[] Radar4Results = new int[2]{4016,4011};
	private int[] Radar5Results = new int[2]{4018,4011};
	
	private int[] isRadar1Fusion = new int[2]{4013,4011};
	

	public int[] getRadar1Results(){
		return Radar1Results;
	}
	
	public int[] getRadar2Results(){
		return Radar2Results;
	}
	
	public int[] getRadar3Results(){
		return Radar3Results;
	}
	
	public int[] getRadar4Results(){
		return Radar4Results;
	}
	
	public int[] getRadar5Results(){
		return Radar5Results;
	}
	
	public int[] getisRadar1Fusion(){
		return isRadar1Fusion;
	}
}
	//以下端口都是根据发送方来控制接收方的数据，例如radar1将数据发送给Fusion，那么命名方式就是【radar1IP,FusionIP】