//const int JOYSTICK_BUTTON_PIN =  2;
//const int          X_AXIS_PIN = A1;
//const int          Y_AXIS_PIN = A0;
//
const int joystickClick = 2;
const int joystickX = A1;
const int joystickY = A0;

const int jumpPin = 8;
const int dashPin = 7;

bool isJumping = false;
bool lastJump = false;

void setup() {
  Serial.begin( 9600 );
  pinMode(8, INPUT);
  pinMode(7, INPUT);
}

void loop() {

  isJumping = digitalRead(jumpPin);
    
  Serial.print(analogRead(joystickX), DEC);
  Serial.print(",");
  Serial.print(analogRead(joystickY), DEC);
  Serial.print(",");
  Serial.print(digitalRead(joystickClick));
  Serial.print(",");
  Serial.print(isJumping != lastJump);
  Serial.print(",");
  Serial.print(digitalRead(dashPin));
  Serial.print("\n");
  delay(15);

  lastJump = isJumping;
}
