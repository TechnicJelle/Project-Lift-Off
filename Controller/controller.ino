#include <Keyboard.h>

const int joystickClick = 2;
const int joystickX = A3;
const int joystickY = A2;
//const int joystickClick = 2;
//const int joystickX = A1;
//const int joystickY = A0;

const int jumpPin = 7;
const int dashPin = 8;
const int attackPin = 6;

void setup() {
  delay(3000);
  Serial.begin( 9600 );
  pinMode(jumpPin, INPUT);
  pinMode(dashPin, INPUT);
  pinMode(attackPin, INPUT);

  Keyboard.begin();
}

void readJoystick() {
  Serial.print(analogRead(joystickX), DEC);
  Serial.print(",");
  Serial.print(analogRead(joystickY), DEC);
  Serial.print(",");
  Serial.print(digitalRead(joystickClick));
  Serial.print("\n");
  delay(15);
}

void readClicks() {
  if(digitalRead(jumpPin) == HIGH) {
    Keyboard.press('w');
  } else if(digitalRead(dashPin) == HIGH) {
    Keyboard.press(KEY_LEFT_SHIFT);
  } else if(digitalRead(attackPin) == HIGH) {
  Keyboard.press('e');
//    Serial.print('e');    
  } else {
    Keyboard.releaseAll();
  }
}

void loop() {
  readJoystick();
  readClicks();
}
