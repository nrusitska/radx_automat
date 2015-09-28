using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;


namespace RadXAutomat.Audio
{
    public class AudioManager
    {
        public void Test()
        {
            var synth = new SpeechSynthesizer();
            synth.SelectVoiceByHints(VoiceGender.Male, VoiceAge.Adult);
            synth.SpeakSsml("<speak version='1.0' xml:lang='de'> <s>halli hallo</s> <s xml:lang='en'>Jack Bones </s></speak>");
        }
    }
}
