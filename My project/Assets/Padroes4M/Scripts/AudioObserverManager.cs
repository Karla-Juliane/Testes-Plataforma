using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
// é o nosso youtube de coisas de audio
public static class AudioObserverManager 
{
   // criar o ponto que permite aos interessados se inscreverem no canal

   public static event Action<float>OnVolumeSliderValueChanged;
   
   //permite que o criador do conteudo do valor do slider mande os dados para o canal
   public static void ChangeVolumeSliderValue(float sliderValue)
   {
      // tem algum inscrito? se tiver mande as notificacoes
      OnVolumeSliderValueChanged?.Invoke(sliderValue);
   }
}
