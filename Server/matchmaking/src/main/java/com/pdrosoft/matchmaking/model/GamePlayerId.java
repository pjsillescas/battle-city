package com.pdrosoft.matchmaking.model;

import java.io.Serializable;

import jakarta.persistence.Embeddable;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

@Embeddable
@Data
@NoArgsConstructor
@AllArgsConstructor
public class GamePlayerId implements Serializable {
	private static final long serialVersionUID = 9070696399080203274L;

	private Long gameId;
	private Long playerId;
}
