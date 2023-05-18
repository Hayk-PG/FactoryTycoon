

public interface IMachineUIValues
{
    event System.Action<float, float> SetSliderLimitationEvent;

    TransformBySlider Slider { get;}

    float MinLimit { get; set; }
    float MaxLimit { get; set; }
}
