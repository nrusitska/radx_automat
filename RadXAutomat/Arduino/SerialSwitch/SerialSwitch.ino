
static const int DEBUG = 0;
struct Input{int pos; int state; };
static Input inputs[] = {
{26,0}, {28,0},
{30,0}, {32,0},
{34,0}, {36,0},
{38,0}, {40,0},
{42,0}, {44,0},
{46,0}, {48,0},
{50,0}, {52,0}
};
static int inputs_length = sizeof(inputs) / sizeof(Input);

void setup()
{
  Serial.begin(115200);
  Serial.flush();
  for(int i=0;i<inputs_length;i++)
  {
    pinMode(inputs[i].pos,INPUT);
  }
  /*for(int i=0;i<inputs_length;i++)
  {
    pinMode(inputs[i].pos,INPUT);
  }*/
}

static char serBuffer[] = {0,0};
void loop()
{
  for(int i=0;i<inputs_length;i++)
  {
    int state = digitalRead(inputs[i].pos);
    if(DEBUG)
      digitalWrite(i, state);
    //entprelen
    if((state != inputs[i].state))
    {
      delay(2);
      state = digitalRead(inputs[i].pos);
    }
    //Änderung per Serial raussenden.
    if((state != inputs[i].state))
    {
      Serial.println("Button:"+String((int)i)+" is: "+String(state));
      Serial.flush();
    }
    inputs[i].state = state;
  }
  Serial.flush();
  /*while( Serial.available() >= 2)
  {
    Serial.readBytes(serBuffer,2);
     Serial.println(String((int)serBuffer[0]) + " set to " + String((int)serBuffer[1]));
    if(serBuffer[1] > 0){
      digitalWrite(serBuffer[0],HIGH);
    }
     else
     {
       digitalWrite(serBuffer[0],LOW);
     }
  }*/
}

