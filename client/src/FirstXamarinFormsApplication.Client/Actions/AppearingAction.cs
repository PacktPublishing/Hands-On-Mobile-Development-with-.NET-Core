using System;
using Xamarin.Forms;

namespace FirstXamarinFormsApplication.Client.Actions
{
    public class AppearingAction : TriggerAction<VisualElement>
    {
        public AppearingAction() { }

        public int StartsFrom { set; get; }

        protected override void Invoke(VisualElement visual)
        {
            visual.Animate("FadeIn", new Animation((opacity) => visual.Opacity = opacity, 0, 1),
            length: 1000, // milliseconds
            easing: Easing.Linear);
        }
    }

public class ChangeStateAction : TriggerAction<VisualElement>
{
    public ChangeStateAction() { }

    public string State { set; get; }

    protected override void Invoke(VisualElement visual)
    {
        if(visual.HasVisualStateGroups())
        {
            VisualStateManager.GoToState(visual, State);
        }
    }
}
}
