\ hd44780_lcd.fs

: qqstimex 384 \ 900
  #, for
      msec
  next
; \ 900 milliseconds


\ 100 us - only instance upstream

: us ( c -- )
  for tusec next
;

: ms ( c -- ) \ good as-is
  \ msec  sleep us(1000)
  for msec next
;

: .E   1 #, ; ( -- n )  \ port pin GP16 Enable
: .RS  2 #, ; ( -- n )  \ port pin GP17 RS

: clr clrmask ;
: setb setmask ;

: gpm gpio_put_masked ; ( mask value -- ) \ alias
: orl $3f #, gpm ; ( mask -- )
: anl invert 0 #,  gpm ; ( mask -- )

: instruction
  \  initGPIO
  .RS clr ;

: data
  \ initGPIO
  .RS setb ;

: dark instruction ;
: bright data ;

: pulseout-E  (  - )
  .E setb
   600 #, ms
  .E clr
;

: poute pulseout-E ;

: clrbits ( -- )
  $3f #, clrmask
;



: xxw_rite-lcd  ( c - )
  $3d #, $0 #, gpio_put_masked  \ %000010 pins anl
  $3d #, clrmask \ reset all but the one bit
  $2  #, setmask \ Instruction mode, clear data bus.
  dup
  $3  #, clrmask \ clear control bits
  setmask \ TOS written to data bits
  drop \ balance stack kludge
;

: wait
  400 #, ms
;

: pins 1 #, drop ; \ syntax sugar

: write-lcd  ( c - )
        $2 #, anl \ .RS setb \ data wrpsForth  four data bit set to zero the 11 is USART
	dup \ both wrpsForth and amrForth - for split nybble
        $f0 #, and \ reset TOS lower four bits wrpsForth

        2/ 2/ orl \ right-shift data wrpsForth
	pulseout-E
	$10 #, * \ left-shift nybble
        $2 #, anl \ instruction wrpsForth  four data bit set to zero the 11 is USART
        2/ 2/ orl \ wrpsForth
	pulseout-E
	;

: init-lcd  (  - )
\ - Also based on working BASIC code.
        $3f #, 0 #, gpm
        initGPIO
	200 #, ms
        $3f #, $c #, gpm
	pulseout-E 100 #, ms
	pulseout-E 100 #, ms
	pulseout-E 100 #, ms
        $3f #, $8 #, gpm
	pulseout-E 100 #, ms
	$28 #, write-lcd 100 #, ms
	$0e #, write-lcd 10 #, ms
	$01 #, write-lcd 100 #, ms
	$02 #, write-lcd 10 #, ms
	data
	;

: init $63 #, negate dup 1+ dup 1+ cr .s cr ;

: wwaa
  $2d #, write-lcd cr \ - minus
  $2d #, write-lcd cr \ - minus
  $2d #, write-lcd cr \ - minus
;

: wwbb
  $20 #, write-lcd cr \   space
  $20 #, write-lcd cr \   space
  $57 #, write-lcd cr \   'W'
;

: wwcc
  $6f #, write-lcd cr \   'o'
  $6b #, write-lcd cr \   'k'
  $77 #, write-lcd cr \   'w'
  $69 #, write-lcd cr \   'i'
;

: wwdd
  $20 #, write-lcd cr \   space
  $20 #, write-lcd cr \   space
  $2d #, write-lcd cr \ - minus
  $2d #, write-lcd cr \ - minus
  $2d #, write-lcd cr \ - minus
;

\ : signoff cr ." That was a demo " cr ;
: signoff 1 #, drop ;

: wwee
    1 #, \ 2^0
    dup  \ 1 -- 1 1
    2*   \ 1 1 -- 1 2
    dup  \ 1 2 -- 1 2 2
    2*   \ 1 2 2 -- 1 2 4
    dup  \ 1 2 4 -- 1 2 4 4
    2*   \ 1 2 4 4 -- 1 2 4 8
    dup  \ 1 2 4 8 -- 1 2 4 8 8
    2*   \ 1 2 4 8 8 -- 1 2 4 8 16
    +    \ 1 2 4 8 16 -- 1 2 4 24
    +    \ 1 2 4 24 -- 1 2 28
    +    \ 1 2 28 -- 1 30
    +    \ 1 30 -- 31

   anl \ clear LEDs
   3 #, for wait next

   3 #, for
     $3f #, clrmask
     7 #, for wait next
     $3f #, setmask
     7 #, for wait next
   next

   $3f #, clrmask \ make dark for the next set:
     14 #, for wait next

   1 #, setb  wait 2 #, setb wait 4 #, setb wait 8 #, setb wait
   16 #, setb wait 32 #, setb 3 #, for wait next
   1 #, clr   wait 2 #, clr  wait 4 #, clr wait  8 #, clr  wait
   16 #, clr  wait 32 #, clr 3 #, for wait next
   $2d #, emit
   $2d #, emit
   $2d #, emit
   .s cr
;

: wwzz wwaa wwbb wwcc wwdd ;

: wokwi \ 2 #, speedy ! 
  init init-lcd 1 #, drop
  wwzz \ main payload
  wwee signoff ; \ signed ;

: tell ." Sun 10 Apr 13:25:49 UTC 2022" cr ;

\  END.
