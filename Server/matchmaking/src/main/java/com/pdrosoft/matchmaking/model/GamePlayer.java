package com.pdrosoft.matchmaking.model;

import jakarta.persistence.Column;
import jakarta.persistence.EmbeddedId;
import jakarta.persistence.Entity;
import jakarta.persistence.ManyToOne;
import jakarta.persistence.MapsId;
import lombok.Data;

@Entity
@Data
public class GamePlayer {

	@EmbeddedId
	private GamePlayerId id = new GamePlayerId();

	@ManyToOne
	@MapsId("gameId")
	private Game game;

	@ManyToOne
	@MapsId("playerId")
	private Player player;

	@Column(nullable = false)
	private String role; // "creator" or "joined"
}
