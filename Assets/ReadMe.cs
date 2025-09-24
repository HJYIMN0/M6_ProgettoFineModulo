

//DOING:
//Sistema di monete
//PlayerParticlesSystem
//Ampliando il sistema Bullet + Enemy aggiugnendo SO e classi astratte

//TO DO:
//AGGIUNGERE UN GAME MANAGER CHE GESTISCA IL LIVELLO, L'AUDIO E ALTRO
//Finire la logica di winning trigger aggiungendo il raccoglitore di monete
//Completare il sistema di salvataggio con il vector 3
//Aggiustare il sistema di raccolta monete con un CoinManager
//Ricontrollare sistema sconfitta e vittoria, ho visto che c'era un problema col prefab e non ho riprovato
//I.e. Aggiungi una linea che con input.getbutton fa comparire il fade in
//Correzione bullet logic
//Implementare logica nuovi nemici e trappole
//Implementare logica piattaforme moventi


// Ho aggiunto i seguenti script:
//AbstractFader
//CanvasGroupFader
//GenericSingleton
//SaveSystem
//SaveData
//Save_Monobehaviour
//TimeManager
//Player Input
//PlayerJumpController
//PlayerParticleSystem
//AsbtractEnemy
//SO_EnemyData
//SO_BulletData


//Ho modificato i seguenti script:
//TimeManager, rendendolo un singleton
//CoinSystem, aggiungendo il CoinManager
//Tolto Lo script dalla camera, sostituendolo con CineMachine
//PlayerController, Separandolo in 3 differenti scirpts
//PlayerAnimatorController, correggendolo e migliorandolo
//Enemy, facendolo derivare da abstractEnemy e trasformandolo in EnemyStationary


//Critiche e miglioramenti proposti dai profs:

//il GameOver canvas non risponde alla pressione dei tasti perch� viene coperto dal WinningScenario (anche se trasparente)

//siccome hai messo [SerializeField] protected Bullet _bulletPreFab { get; private set; }
//hai trasformato questa variabile in una property ed � per questo che Unity non la serializza
//e quindi non hai potuto assegnare il prefab da spawnare

//attenzione a non normalizzare l'input del player se ha magnitudo < 1 perch� altrimenti parte subito veloce e
//quando si ferma si ferma in ritardo (senza rallentare)

//non dovresti tenere i canvas sempre tutti attivi

//inoltre non � una buona pratica mettere Time.timeScale = 0 dentro un update come nel caso del
//GameOver_UI perch� diventa molto difficile poi andare a controllare il flusso, ad esempio il gioco a volte
//mi si blocca probabilmente per un timeScale = 0 che parte da qualche parte

//le animazioni non avevano l'avatar settato correttamente (solo il modello principale lo aveva,
//gl ialtri dovevano fare copy from other avatar)

//attenzione alla struttura del Player, le cose di logica dovrebbero essere nella root,
//inoltre il GetComponentInChildren pescava l'_animator sbagliato, perch� inizia a cercare dal GameObject stesso

//nell' OnCollisionExit() del MovingPlatform _playerRb.velocity = _playerRb.velocity; non fa nulla

//il livello non � completabile (il salto prima del cartellone che dice di correre non si pu� fare)

//_anim.SetBool(""IsJumping"", true); era messo a true anche nel branch in cui doveva essere false,
//in questi casi � meglio scrivere con una riga sola _anim.SetBool(""IsJumping"", _player._isRunning);

//inoltre, attenzione, non mettere variabili public o property con get public che inizino con _
//quello serve a indicare una variabile visibile solo dentro la classe, al massimo dentro le sottoclassi!

//infine per gestire le animazioni sarebbe meglio fare una cosa pi� simile all'esempio visto a lezione:
//un blend per la fase di ground tra idle/walk/run, un blend per la fase aerea (ascending/mid/descending)
//un'animazione per il salto (tramite trigger) e una di landing (tramite booleana) per uscire dalla fase aerea"

