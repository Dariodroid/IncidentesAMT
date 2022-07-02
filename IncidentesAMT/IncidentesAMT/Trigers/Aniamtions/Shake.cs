using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace IncidentesAMT.Trigers.Aniamtions
{
    public class Shake : TriggerAction<Xamarin.Forms.View>
    {
        protected override async void Invoke(Xamarin.Forms.View sender)
        {
            uint time = 50;
            await sender.TranslateTo(-15,0,time);
            await sender.TranslateTo(15, 0, time);
            await sender.TranslateTo(-10, 0, time);
            await sender.TranslateTo(10, 0, time);
            await sender.TranslateTo(-5, 0, time);
            await sender.TranslateTo(5, 0, time);

            sender.TranslationX = 0;
        }
    }
}
