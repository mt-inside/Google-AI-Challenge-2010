#!/bin/bash

BOTDIR=bots
MAPDIR=maps
LOGDIR=log
REPLAYDIR=replay

mkdir $LOGDIR
mkdir $REPLAYDIR

BOTS=""
#for b in $BOTDIR/*.jar $BOTDIR/*.exe
#for b in $BOTDIR/*.exe
for b in $BOTDIR/*.jar
do
    BOTS+="`basename $b` "
done
#BOTS="BullyBot.jar"

MAPS=""
for n in `seq 1 100`
do
    MAPS+="map${n}.txt "
done
#MAPS="map1.txt"

ENGINE="java -jar ../../runtime/PlayGame.jar"

MTBOT="../../build/bin/Debug/mtBot.exe"

REPS=1
MAXTIME=1000
MAXTURNS=1000

STDERRFILE="stderr.txt"
WINLOSSFILE="winloss.txt"
WINLOSSTMP="winloss.tmp.txt"

function play_game
{
    local MAP=$1
    local PLAYER1=$2
    local PLAYER2=$3
    local LOG=$4
    local REPLAY=$5

    $ENGINE "$MAP" $MAXTIME $MAXTURNS "$LOG" "${PLAYER1}" "${PLAYER2}" > "$REPLAY" 2> "$STDERRFILE"
}

for bot in $BOTS
do
    GAMECOUNT=0

    rm "$WINLOSSTMP"

    for map in $MAPS
    do
        for rep in $REPS
        do
            LOGFILE="$LOGDIR/log_${bot}_${map}_${rep}.txt"

            if [ ! -z `echo $bot | grep .jar` ]
            then
                play_game "$MAPDIR/$map" "$MTBOT" "java -jar $BOTDIR/$bot" $LOGFILE "$REPLAYDIR/replay_${bot}_${map}_${rep}.txt"
            else
                play_game "$MAPDIR/$map" "$MTBOT" "$BOTDIR/$bot" $LOGFILE "$REPLAYDIR/replay_${bot}_${map}_${rep}.txt"
            fi


            echo -n "$bot $map $rep " >> $WINLOSSFILE
            cat $STDERRFILE | grep "Player . Wins" >> $WINLOSSFILE
            cat $STDERRFILE | grep "Player . Wins" >> $WINLOSSTMP

            if [ ! -z "`cat $LOGFILE | grep "^Dropping"`" ]
            then
                echo "DROPPED! Against $bot on $map rep $rep"
                cat $LOGFILE | grep "^Dropping"
                exit 1
            fi

            if [ ! -z "`cat $STDERRFILE | grep "crashed"`" ]
            then
                echo "CRASHED! Against $bot on $map rep $rep"
                #exit 1
            fi

            if [ ! -z "`cat $STDERRFILE | grep "timed out"`" ]
            then
                echo "TIMED OUT! Against $bot on $map rep $rep"
                #exit 1
            fi

            #if [ ! -z "`cat $STDERRFILE | grep "WARNING"`" ]
            #then
                #echo "UNKNOWN WARNING! Against $bot on $map rep $rep"
                #cat $STDERRFILE | grep "WARNING"
                #exit 1
            #fi

            GAMECOUNT=$((GAMECOUNT + 1))
        done
    done

    echo -n $bot
    STATS=`cat $WINLOSSTMP | grep "Player 1 Wins" | uniq -c | sed 's/\s*\([0-9]*\).*/\1/'`
    [ -z "$STATS" ] && STATS=0
    echo "  $STATS / $GAMECOUNT ( `echo "scale=0; 100 * $STATS / $GAMECOUNT" | bc`% )"
done
